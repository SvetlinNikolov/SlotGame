namespace SlotGame.UnitTests.ServicesTests;

using NSubstitute;
using SlotGame.Domain.Constants;
using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Enums;
using SlotGame.Services;
using SlotGame.Services.Contracts;

public class SlotMachineServiceTests
{
    private readonly ISpinResultService _spinResultService = Substitute.For<ISpinResultService>();
    private readonly SlotMachineService _slotMachineServiceMock;
    private readonly SlotMachineService _slotMachineService;

    public SlotMachineServiceTests()
    {
        _slotMachineServiceMock = new SlotMachineService(_spinResultService);

        var randomService = Substitute.For<IRandomService>();
        var realSpinService = new SpinResultService(randomService);
        _slotMachineService = new SlotMachineService(realSpinService);
    }

    [Theory]
    [InlineData(BetConstants.MinBet - 1)]
    [InlineData(BetConstants.MaxBet + 1)]
    public void Spin_ShouldFail_WhenBetIsBelowMinimumOrAboveMaximum(decimal bet)
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());
        wallet.Deposit(100);

        // Act
        var result = _slotMachineService.Spin(wallet, bet);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.BetAmountNotInValidRange().Message, result.Error?.Message);
    }

    [Fact]
    public void Spin_ShouldFail_WhenWithdrawalFails()
    {
        var wallet = new Wallet(Guid.NewGuid());
        var betAmount = 5;

        var result = _slotMachineServiceMock.Spin(wallet, betAmount);

        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.InsufficientBalance().Message, result.Error?.Message);
    }

    [Fact]
    public void Spin_ShouldRefund_WhenSpinFails()
    {
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 645;
        var betAmount = 5;
        wallet.Deposit(depositAmount);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Failure(SlotGameErrors.UnknownSpinResult()));

        var result = _slotMachineServiceMock.Spin(wallet, betAmount);

        Assert.True(result.IsFailure);
        Assert.Equal(depositAmount, wallet.Balance);
    }

    [Fact]
    public void Spin_ShouldSucceed_Loss()
    {
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 21;
        var betAmount = 8;
        var multiplier = 0;
        var expectedBalance = depositAmount - betAmount;
        wallet.Deposit(depositAmount);

        var spin = new SpinResult(SpinOutcome.Loss, betAmount, multiplier);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Success(spin));

        var result = _slotMachineServiceMock.Spin(wallet, betAmount);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, wallet.Balance);
    }

    [Fact]
    public void Spin_ShouldSucceed_OnWin()
    {
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 44;
        var betAmount = 2;
        var multiplier = 5;
        var expectedBalance = depositAmount - betAmount + betAmount * multiplier;
        wallet.Deposit(depositAmount);

        var spin = new SpinResult(SpinOutcome.Win, betAmount, multiplier);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Success(spin));

        var result = _slotMachineServiceMock.Spin(wallet, betAmount);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, wallet.Balance);
    }

    [Fact]
    public void Spin_ShouldSucceed_OnBigWin()
    {
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 10;
        var betAmount = 5;
        var multiplier = 6;
        var expectedBalance = depositAmount - betAmount + betAmount * multiplier;
        wallet.Deposit(depositAmount);

        var spin = new SpinResult(SpinOutcome.BigWin, betAmount, multiplier);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Success(spin));

        var result = _slotMachineServiceMock.Spin(wallet, betAmount);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, wallet.Balance);
    }
}
