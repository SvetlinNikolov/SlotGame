namespace SlotGame.UnitTests.ServicesTests;

using NSubstitute;
using SlotGame.Domain.Constants;
using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Enums;
using SlotGame.Services;
using SlotGame.Services.Contracts;

public class SpinResultServiceTests
{
    private readonly IRandomService _randomService = Substitute.For<IRandomService>();
    private readonly SpinResultService _service;

    public SpinResultServiceTests()
    {
        _service = new SpinResultService(_randomService);
    }

    [Theory]
    [InlineData(1.0, SpinOutcome.Loss, 0.0, 5.0, 0.0)]
    [InlineData(51.0, SpinOutcome.Win, 1.5, 10.0, 15.0)]
    [InlineData(91.0, SpinOutcome.BigWin, 2.5, 4.0, 10.0)]
    public void GetSpinResult_ShouldReturnCorrectOutcome(
     decimal roll, SpinOutcome expectedOutcome, decimal multiplier, decimal betAmount, decimal expectedTotalWin)
    {
        // Arange
        _randomService.GetRandomDecimal(1, BetConstants.TotalPercent).Returns(roll);

        if (expectedOutcome == SpinOutcome.Win)
        {
            _randomService
                .GetRandomDecimal(BetConstants.BaseWinMinMultiplier, BetConstants.BaseWinMaxMultiplier)
                .Returns(multiplier);
        }
        else if (expectedOutcome == SpinOutcome.BigWin)
        {
            _randomService
                .GetRandomDecimal(BetConstants.BigWinMinMultiplier, BetConstants.BigWinMaxMultiplier)
                .Returns(multiplier);
        }

        // Act
        var result = _service.GetSpinResult(betAmount);

        // Assert
        Assert.True(result.IsSuccess);
        var data = result.GetData<SpinResult>();
        Assert.Equal(expectedOutcome, data.SpinOutcome);
        Assert.Equal(multiplier, data.Multiplier);
        Assert.Equal(expectedTotalWin, data.TotalWin);
        Assert.Equal(betAmount, data.BetAmount);
    }



    [Fact]
    public void GetSpinResult_ShouldReturnFailure_WhenRollIsOutsideRange()
    {
        // Arange
        _randomService.GetRandomInt(1, BetConstants.TotalPercent)
            .Returns(BetConstants.TotalPercent + 1); // invalid roll

        // Act
        var result = _service.GetSpinResult(10);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.UnknownSpinResult().Message, result.Error?.Message);
    }

    [Theory]
    [InlineData(BetConstants.MinBet - 1)]
    [InlineData(BetConstants.MaxBet + 1)]
    public void GetSpinResult_ShouldReturnFailure_WhenBetAmountIsOutOfRange(decimal invalidBet)
    {
        // Act
        var result = _service.GetSpinResult(invalidBet);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.BetAmountNotInValidRange().Message, result.Error?.Message);
    }
}
