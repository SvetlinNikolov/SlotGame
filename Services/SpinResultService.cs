namespace SlotGame.Services;

using SlotGame.Domain.Constants;
using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Enums;
using SlotGame.Services.Contracts;

public class SpinResultService : ISpinResultService
{
    private readonly IRandomService _randomService;

    public SpinResultService(IRandomService randomService)
    {
        _randomService = randomService;
    }

    public Result GetSpinResult(decimal betAmount)
    {
        if (betAmount < GlobalConstants.MinBet || betAmount > GlobalConstants.MaxBet)
        {
            return Result.Failure(SlotGameErrors.BetAmountNotInValidRange());
        }

        var spinOutcomeResult = CalculateSpinOutcome();
        if (spinOutcomeResult.IsFailure)
        {
            return spinOutcomeResult;
        }

        var spinOutcome = spinOutcomeResult.GetData<SpinOutcome>();

        var multiplier = spinOutcome switch
        {
            SpinOutcome.Loss => 0,
            SpinOutcome.Win => _randomService.GetRandomDecimal(GlobalConstants.BaseWinMinMultiplier, GlobalConstants.BaseWinMaxMultiplier),
            SpinOutcome.BigWin => _randomService.GetRandomDecimal(GlobalConstants.BigWinMinMultiplier, GlobalConstants.BigWinMaxMultiplier),
            _ => 0,
        };

        var result = new SpinResult(spinOutcome, betAmount, multiplier);
        return Result.Success(result);
    }

    private Result CalculateSpinOutcome()
    {
        var roll = _randomService.GetRandomDecimal(1, GlobalConstants.TotalPercent);
        int cumulative = 0;

        foreach (var (outcome, percent) in GamePercentageConfig.GamePercentages)
        {
            cumulative += percent;

            if (roll <= cumulative)
            {
                return Result.Success(outcome);
            }
        }

        return Result.Failure(SlotGameErrors.UnknownSpinResult());
    }
}
