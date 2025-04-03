using SlotGame.Enums;

namespace SlotGame;

public class GameHistory
{
    public int PlayerId { get; set; }

    public decimal BetAmount { get; set; }

    public decimal WinAmount { get; set; }

    public BetResult BetResult { get; set; }
}
