using SlotGame.Enums;

namespace SlotGame.Domain.Models;

public class SpinResult
{
    public SpinResult(SpinOutcome spinOutcome, decimal baseWin, decimal multiplier)
    {
        SpinOutcome = spinOutcome;
        BaseWin = baseWin;
        Multiplier = multiplier;
    }

    public SpinOutcome SpinOutcome { get; }

    public decimal BaseWin { get; }

    public decimal Multiplier { get; }

    public decimal TotalWin => BaseWin * Multiplier;
}
