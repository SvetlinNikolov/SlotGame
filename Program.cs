using Microsoft.Extensions.DependencyInjection;
using SlotGame.Factories;
using SlotGame.Factories.Contracts;
using SlotGame.Services;
using SlotGame.Services.Contracts;

namespace SlotGame;

public class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IPlayerFactory, PlayerFactory>()
            .AddSingleton<IWalletFactory, WalletFactory>()
            .AddSingleton<IGameService, GameService>()
            .BuildServiceProvider();

        var game = serviceProvider.GetRequiredService<IGameService>();
        game.Run();
    }
}
