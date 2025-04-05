using SlotGame.Domain.Constants;
using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Enums;
using SlotGame.Services.Contracts;

namespace SlotGame.Services;

public class SlotMachineService(ISpinResultService spinResultService) : ISlotMachineService
{
    public Result Spin(Wallet wallet, decimal betAmount)
    {
        if (betAmount < BetConstants.MinBet || betAmount > BetConstants.MaxBet)
        {
            return Result.Failure(SlotGameErrors.BetAmountNotInValidRange());
        }

        var withdrawResult = wallet.Withdraw(betAmount);

        if (withdrawResult.IsFailure)
        {
            return withdrawResult;
        }

        var spinResult = spinResultService.GetSpinResult(betAmount);

        if (spinResult.IsFailure)
        {
            wallet.Deposit(betAmount); // nice to have, if spin fails we return the player's bet amount
            return spinResult;
        }

        var spinData = spinResult.GetData<SpinResult>();
        wallet.Deposit(spinData.TotalWin);

        var message = spinData.SpinOutcome == SpinOutcome.Win || spinData.SpinOutcome == SpinOutcome.BigWin
            ? $"Congrats - you won ${spinData.TotalWin:F2}! Your current balance is: ${wallet.Balance:F2}"
            : $"No luck this time! Your current balance is: ${wallet.Balance:F2}";

        return Result.Success(message);
    }
}
