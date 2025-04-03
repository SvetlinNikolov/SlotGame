namespace SlotGame;

public class Wallet
{
    public int Id { get; set; } // Not needed for in memory but needed if we use db

    public int PlayerId { get; set; }

    public decimal Balance { get; private set; }
}
