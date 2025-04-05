using SlotGame.Domain.Constants;
using System;

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
           => new($"Bet amount must be between ${GlobalConstants.MinBet} and ${GlobalConstants.MaxBet}");

        public static Error UnknownSpinResult()
          => new($"Spin result could not be determined.");
    }
}
