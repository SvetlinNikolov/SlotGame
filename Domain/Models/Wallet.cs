namespace SlotGame.Domain.Models;

public class Wallet(Guid playerId)
{
    public Guid Id { get; } = Guid.NewGuid();

    public Guid PlayerId { get; } = playerId;

    public decimal Balance { get; private set; } = 0; // Could also use constant or leave it as it is.
}
