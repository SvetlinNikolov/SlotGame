namespace SlotGame.Domain.Errors
{
    public class SlotGameErrors
    {
        public static Error DuplicatePlayerWalletError(Guid playerId)
            => new($"Player with Id {playerId} already has a wallet. Each player can have only one wallet.");
    }
}
