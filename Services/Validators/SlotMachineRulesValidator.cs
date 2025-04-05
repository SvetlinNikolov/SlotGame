using SlotGame.Domain.Constants;

namespace SlotGame.Services.Validators;

public static class SlotMachineRulesValidator
{
    public static void ValidateGamePercentages()
    {
        int total = GamePercentageConfig.GamePercentages.Values.Sum();

        if (total != GlobalConstants.TotalPercent)
        {
            throw new InvalidOperationException($"Config Error: Game percentages must total {GlobalConstants.TotalPercent}%, but got {total}%.");
        }
    }
}
