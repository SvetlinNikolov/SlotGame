namespace SlotGame;

public class Player
{
    public int Id { get; set; }

    public int WalletId { get; set; }

    public Wallet Wallet { get; set; } = new();
}
