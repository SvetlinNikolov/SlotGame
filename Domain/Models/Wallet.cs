namespace SlotGame.Domain.Models;

using Result;
using SlotGame.Domain.Errors;

public class Wallet(Guid playerId)
{
    public Guid Id { get; } = Guid.NewGuid();

    public Guid PlayerId { get; } = playerId;

    public decimal Balance { get; private set; }

    public Result Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Failure(SlotGameErrors.InvalidDepositAmount());
        }

        Balance += amount;
        return Result.Success($"Your deposit of ${amount} was successful. Your current balance is: ${Balance}");
    }

    public Result Withdrawal(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Failure(SlotGameErrors.InvalidWithdrawalAmount());
        }
        if (Balance - amount < 0)
        {
            return Result.Failure(SlotGameErrors.InsufficientBalance());
        }

        Balance -= amount;
        return Result.Success($"Your withdrawal of ${amount} was successful. Your current balance is: ${Balance}");
    }
}
