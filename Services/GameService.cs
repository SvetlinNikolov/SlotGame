using SlotGame.Domain.Models;
using SlotGame.Enums;
using SlotGame.Factories;
using SlotGame.Factories.Contracts;
using SlotGame.Helpers;
using SlotGame.Services.Contracts;

namespace SlotGame.Services
{
    public class GameService(IWalletFactory walletFactory, IPlayerFactory playerFactory) : IGameService
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

            // Main loop
            while (true)
            {
                ConsoleHelper.PrintInfo("Please, submit action:");
                var (action, arg) = ActionHelper.Parse(Console.ReadLine())!.Value;

                if (arg is null && action is not GameAction.Exit)
                {
                    ConsoleHelper.PrintError("This action requires an amount.");
                    continue;
                }

                switch (action)
                {
                    case GameAction.Deposit:
                        var depositResult = wallet.Deposit(arg.Value); // not good value call.
                        if (depositResult.IsFailure)
                        {
                            ConsoleHelper.PrintError(depositResult.Error!);
                            continue;
                        }
                        ConsoleHelper.PrintInfo(depositResult.GetData<string>());
                        break;
                    case GameAction.Withdraw:
                        var withdrawalResult = wallet.Withdrawal(arg.Value);
                        if (withdrawalResult.IsFailure)
                        {
                            ConsoleHelper.PrintError(withdrawalResult.Error!);
                            continue;
                        }
                        ConsoleHelper.PrintInfo(withdrawalResult.GetData<string>());
                        break;
                }
            }
        }
    }
}
