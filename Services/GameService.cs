using SlotGame.Domain.Models;
using SlotGame.Enums;
using SlotGame.Factories.Contracts;
using SlotGame.Helpers;
using SlotGame.Services.Contracts;

namespace SlotGame.Services
{
    public class GameService(IWalletFactory walletFactory, IPlayerFactory playerFactory, ISlotMachineService slotMachineService) : IGameService
    {
        public void Run()
        {
            var player = playerFactory.CreatePlayer();
            var walletResult = walletFactory.CreateWallet(player.Id);

            if (walletResult.IsFailure)
            {
                ConsoleHelper.PrintError(walletResult.Error!);
                return;
            }

            var wallet = walletResult.GetData<Wallet>();
            RunMainLoop(wallet);
        }

        private void RunMainLoop(Wallet wallet)
        {
            while (true)
            {
                ConsoleHelper.PrintInfo("Please, submit action:");
                var (action, arg) = ActionHelper.Parse(Console.ReadLine())!.Value;

                if (arg is null && action is not GameAction.Exit)
                {
                    ConsoleHelper.PrintError("Invalid action!");
                    continue;
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
                    ConsoleHelper.PrintInfo("Thank you for playing! Hope to see you again soon.");
                    Environment.Exit(0);
                    break;
            }
        }

        private void HandleDeposit(Wallet wallet, decimal amount)
        {
            var depositResult = wallet.Deposit(amount);
            if (depositResult.IsFailure)
            {
                ConsoleHelper.PrintError(depositResult.Error!);
                return;
            }

            ConsoleHelper.PrintInfo(depositResult.GetData<string>());
        }

        private void HandleWithdraw(Wallet wallet, decimal amount)
        {
            var withdrawResult = wallet.Withdraw(amount);
            if (withdrawResult.IsFailure)
            {
                ConsoleHelper.PrintError(withdrawResult.Error!);
                return;
            }

            ConsoleHelper.PrintInfo(withdrawResult.GetData<string>());
        }

        private void HandleBet(Wallet wallet, decimal amount)
        {
            var spinResult = slotMachineService.Spin(wallet, amount);
            if (spinResult.IsFailure)
            {
                ConsoleHelper.PrintError(spinResult.Error!);
                return;
            }

            ConsoleHelper.PrintInfo(spinResult.GetData<string>());
        }
    }
}
