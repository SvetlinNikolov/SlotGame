namespace SlotGame.Domain.Errors
{
    public class SlotGameErrors
    {
        public static Error DuplicateWallet(Guid playerId)
            => new($"Player with Id {playerId} already has a wallet. Each player can have only one wallet.");

        public static Error InvalidDepositAmount()
            => new("Deposit amount must be greater than zero.");

        public static Error InvalidWithdrawalAmount()
            => new("Withdrawal amount must be greater than zero.");

        public static Error InsufficientBalance()
            => new("Insufficient balance");
    }
}
