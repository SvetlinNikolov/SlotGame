namespace SlotGame.UnitTests.ServicesTests;

using NSubstitute;
using SlotGame.Domain.Constants;
using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Enums;
using SlotGame.Services;
using SlotGame.Services.Contracts;
using Xunit;

public class SlotMachineServiceTests
{
    private readonly ISpinResultService _spinResultService = Substitute.For<ISpinResultService>();
    private readonly SlotMachineService _service;

    public SlotMachineServiceTests()
    {
        _service = new SlotMachineService(_spinResultService);
    }

    [Theory]
    [InlineData(GlobalConstants.MinBet - 1)]
    [InlineData(GlobalConstants.MaxBet + 1)]
    public void Spin_ShouldFail_WhenBetIsBelowMinimumOrAboveMaximum(decimal bet)
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());

        // Act
        var result = _service.Spin(wallet, bet);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.BetAmountNotInValidRange().Message, result.Error?.Message);
    }

    [Fact]
    public void Spin_ShouldFail_WhenWithdrawalFails()
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());
        var betAmount = 5;

        // Act
        var result = _service.Spin(wallet, betAmount);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.InsufficientBalance().Message, result.Error?.Message);
    }

    [Fact]
    public void Spin_ShouldRefund_WhenSpinFails()
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 645;
        var betAmount = 5;
        wallet.Deposit(depositAmount);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Failure(SlotGameErrors.UnknownSpinResult()));

        // Act
        var result = _service.Spin(wallet, betAmount);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(depositAmount, wallet.Balance);
    }

    [Fact]
    public void Spin_ShouldSucceed_Loss()
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 21;
        var betAmount = 8;
        var multiplier = 0;
        var expectedBalance = depositAmount - betAmount;
        wallet.Deposit(depositAmount);

        var spin = new SpinResult(SpinOutcome.Loss, betAmount, multiplier);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Success(spin));

        // Act
        var result = _service.Spin(wallet, betAmount);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, wallet.Balance);
    }

    [Fact]
    public void Spin_ShouldSucceed_OnWin()
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 44;
        var betAmount = 2;
        var multiplier = 5;
        var expectedBalance = depositAmount - betAmount + betAmount * multiplier;
        wallet.Deposit(depositAmount);

        var spin = new SpinResult(SpinOutcome.Win, betAmount, multiplier);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Success(spin));

        // Act
        var result = _service.Spin(wallet, betAmount);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, wallet.Balance);
    }

    [Fact]
    public void Spin_ShouldSucceed_OnBigWin()
    {
        // Arrange
        var wallet = new Wallet(Guid.NewGuid());
        var depositAmount = 10;
        var betAmount = 5;
        var multiplier = 6;
        var expectedBalance = depositAmount - betAmount + betAmount * multiplier;
        wallet.Deposit(depositAmount);

        var spin = new SpinResult(SpinOutcome.BigWin, betAmount, multiplier);
        _spinResultService.GetSpinResult(betAmount).Returns(Result.Success(spin));

        // Act
        var result = _service.Spin(wallet, betAmount);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, wallet.Balance);
    }
}
