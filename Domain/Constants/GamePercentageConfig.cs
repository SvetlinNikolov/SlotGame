using SlotGame.Enums;

namespace SlotGame.Domain.Constants;

public static class GamePercentageConfig
{
    public static IReadOnlyDictionary<SpinOutcome, int> GamePercentages { get; } = new Dictionary<SpinOutcome, int>
    {
        [SpinOutcome.Loss] = GlobalConstants.LossChancePercent,
        [SpinOutcome.Win] = GlobalConstants.WinChancePercent,
        [SpinOutcome.BigWin] = GlobalConstants.BigWinChancePercent
    };
}
