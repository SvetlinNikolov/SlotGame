using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Enums;
using SlotGame.Factories.Contracts;
using SlotGame.Helpers;
using SlotGame.Services.Contracts;

namespace SlotGame.Services;

public class GameService(IWalletFactory walletFactory, IPlayerFactory playerFactory, ISlotMachineService slotMachineService, IConsoleService consoleService, IApplicationControl applicationControl) : IGameService
{
    public void Run()
    {
        var player = playerFactory.CreatePlayer();
        var walletResult = walletFactory.CreateWallet(player.Id);

        if (walletResult.IsFailure)
        {
            consoleService.PrintError(walletResult.Error!);
            return;
        }

        var wallet = walletResult.GetData<Wallet>();

        RunMainLoop(wallet);
        Exit();
    }

    public void Exit()
    {
        applicationControl.Exit();
    }

    private void RunMainLoop(Wallet wallet)
    {
        while (true)
        {
            consoleService.PrintInfo("Please, submit action:");
            var (action, arg) = ActionHelper.Parse(consoleService.ReadLine())!.Value;

            if (arg is null && action is not GameAction.Exit)
            {
                consoleService.PrintError(SlotGameErrors.InvalidCommand());
                continue;
            }
            if (action == GameAction.Exit)
            {
                return;
            }
            ExecuteAction(action, arg, wallet);
        }
    }

    private void ExecuteAction(GameAction action, decimal? arg, Wallet wallet)
    {
        switch (action)
        {
            case GameAction.Deposit:
                HandleDeposit(wallet, arg!.Value);
                break;

            case GameAction.Withdraw:
                HandleWithdraw(wallet, arg!.Value);
                break;

            case GameAction.Bet:
                HandleBet(wallet, arg!.Value);
                break;

            case GameAction.Exit:
                consoleService.PrintInfo("Thank you for playing! Hope to see you again soon.");
                Exit();
                break;
        }
    }

    private void HandleDeposit(Wallet wallet, decimal amount)
    {
        var depositResult = wallet.Deposit(amount);
        if (depositResult.IsFailure)
        {
            consoleService.PrintError(depositResult.Error!);
            return;
        }

        consoleService.PrintInfo(depositResult.GetData<string>());
    }

    private void HandleWithdraw(Wallet wallet, decimal amount)
    {
        var withdrawResult = wallet.Withdraw(amount);
        if (withdrawResult.IsFailure)
        {
            consoleService.PrintError(withdrawResult.Error!);
            return;
        }

        consoleService.PrintInfo(withdrawResult.GetData<string>());
    }

    private void HandleBet(Wallet wallet, decimal amount)
    {
        var spinResult = slotMachineService.Spin(wallet, amount);
        if (spinResult.IsFailure)
        {
            consoleService.PrintError(spinResult.Error!);
            return;
        }
        var spinData = spinResult.GetData<SpinResult>();

        var message = spinData.SpinOutcome == SpinOutcome.Win || spinData.SpinOutcome == SpinOutcome.BigWin
            ? $"Congrats - you won ${spinData.TotalWin:F2}! Your current balance is: ${wallet.Balance:F2}"
            : $"No luck this time! Your current balance is: ${wallet.Balance:F2}";

        consoleService.PrintInfo(message);
    }
}
