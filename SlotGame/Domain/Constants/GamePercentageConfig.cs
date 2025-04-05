using SlotGame.Enums;

namespace SlotGame.Domain.Constants;

public static class GamePercentageConfig
{
    /// <summary>
    /// Ordered list of outcome probabilities. The order is important for correct spin result evaluation. 
    /// </summary>
    public static readonly List<(SpinOutcome outcome, int percent)> GamePercentages =
 [
    (SpinOutcome.Loss, BetConstants.LossChancePercent),
    (SpinOutcome.Win, BetConstants.WinChancePercent),
    (SpinOutcome.BigWin, BetConstants.BigWinChancePercent),
 ];
}
