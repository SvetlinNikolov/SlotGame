using SlotGame.Domain.Constants;
using SlotGame.Enums;

namespace SlotGame.Services.Validators;

public static class SlotMachineRulesValidator
{
    public static void ValidateGamePercentages()
    {
        var percentages = GamePercentageConfig.GamePercentages;

        if (percentages.Sum(x => x.percent) != BetConstants.TotalPercent)
        {
            throw new InvalidOperationException($"Config Error: Game percentages must total {BetConstants.TotalPercent}%.");

        }

        var expectedOrder = new[] { SpinOutcome.Loss, SpinOutcome.Win, SpinOutcome.BigWin };

        for (int i = 0; i < expectedOrder.Length; i++)
        {
            if (percentages[i].outcome != expectedOrder[i])
            {
                throw new InvalidOperationException("Config Error: GamePercentages must be ordered as Loss → Win → BigWin.");
            }
        }
    }
}
