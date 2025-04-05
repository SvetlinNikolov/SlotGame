namespace SlotGame.Services;

using SlotGame.Domain.Constants;
using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Enums;
using SlotGame.Services.Contracts;

public class SpinResultService(IRandomService randomService) : ISpinResultService
{
    public Result GetSpinResult(decimal betAmount)
    {
        if (betAmount < BetConstants.MinBet || betAmount > BetConstants.MaxBet)
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
            SpinOutcome.Win => randomService.GetRandomDecimal(BetConstants.BaseWinMinMultiplier, BetConstants.BaseWinMaxMultiplier),
            SpinOutcome.BigWin => randomService.GetRandomDecimal(BetConstants.BigWinMinMultiplier, BetConstants.BigWinMaxMultiplier),
            _ => 0,
        };

        var result = new SpinResult(spinOutcome, betAmount, multiplier);
        return Result.Success(result);
    }

    private Result CalculateSpinOutcome()
    {
        var roll = randomService.GetRandomInt(1, BetConstants.TotalPercent);
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
