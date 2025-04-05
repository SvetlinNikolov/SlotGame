using SlotGame.Enums;

namespace SlotGame.Domain.Models;

public class SpinResult
{
    public SpinResult(SpinOutcome spinOutcome, decimal betAmount, decimal multiplier)
    {
        SpinOutcome = spinOutcome;
        BetAmount = betAmount;
        Multiplier = multiplier;
    }

    public SpinOutcome SpinOutcome { get; }

    public decimal BetAmount { get; }

    public decimal Multiplier { get; }

    public decimal TotalWin => BetAmount * Multiplier;
}
