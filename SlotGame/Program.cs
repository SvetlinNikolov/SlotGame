using Microsoft.Extensions.DependencyInjection;
using SlotGame.Factories;
using SlotGame.Factories.Contracts;
using SlotGame.Services;
using SlotGame.Services.Contracts;
using SlotGame.Services.Validators;

namespace SlotGame;

public class Program
{
    static void Main(string[] args)
    {
        SlotMachineRulesValidator.ValidateGamePercentages();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IPlayerFactory, PlayerFactory>()
            .AddSingleton<IWalletFactory, WalletFactory>()
            .AddSingleton<IGameService, GameService>()
            .AddSingleton<ISpinResultService, SpinResultService>()
            .AddSingleton<ISlotMachineService, SlotMachineService>()
            .AddSingleton<IRandomService, RandomService>()
            .AddSingleton<IConsoleService, ConsoleService>()
            .BuildServiceProvider();

        var game = serviceProvider.GetRequiredService<IGameService>();
        game.Run();
    }
}
