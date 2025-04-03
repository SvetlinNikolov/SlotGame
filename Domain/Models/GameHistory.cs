using SlotGame.Enums;

namespace SlotGame.Domain.Models;

public class GameHistory
{
    public Guid PlayerId { get; set; }

    public decimal BetAmount { get; set; }

    public decimal WinAmount { get; set; }

    public BetResult BetResult { get; set; }
}
