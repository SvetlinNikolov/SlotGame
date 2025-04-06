using SlotGame.Domain.Constants;

namespace SlotGame.Domain.Errors
{
    public class SlotGameErrors
    {
        public static Error DuplicateWallet(Guid playerId)
            => new($"Player with Id {playerId} already has a wallet. Each player can have only one wallet.");

        public static Error InvalidDepositAmount()
            => new("Deposit amount must be greater than zero.");

        public static Error InvalidWithdrawAmount()
            => new("Withdraw amount must be greater than zero.");

        public static Error InsufficientBalance()
            => new("Insufficient balance");

        public static Error BetAmountNotInValidRange()
           => new($"Bet amount must be between ${BetConstants.MinBet} and ${BetConstants.MaxBet}");

        public static Error UnknownSpinResult()
          => new($"Spin result could not be determined.");

        public static Error InvalidCommand()
         => new($"Invalid command. Use: deposit <amount>, withdraw <amount>, bet <amount>, or exit.");
    }
}
