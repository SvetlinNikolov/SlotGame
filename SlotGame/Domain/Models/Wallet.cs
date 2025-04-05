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
        amount = Math.Round(amount, 2, MidpointRounding.ToZero);

        if (amount <= 0)
        {
            return Result.Failure(SlotGameErrors.InvalidDepositAmount());
        }

        Balance += amount;
        return Result.Success($"Your deposit of ${amount:F2} was successful. Your current balance is: ${Balance:F2}");
    }

    public Result Withdraw(decimal amount)
    {
        amount = Math.Round(amount, 2, MidpointRounding.ToZero);

        if (amount <= 0)
        {
            return Result.Failure(SlotGameErrors.InvalidWithdrawAmount());
        }

        if (Balance - amount < 0)
        {
            return Result.Failure(SlotGameErrors.InsufficientBalance());
        }

        Balance -= amount;
        return Result.Success($"Your withdraw of ${amount} was successful. Your current balance is: ${Balance:F2}");
    }
}
