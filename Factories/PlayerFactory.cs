using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Factories.Contracts;

namespace SlotGame.Factories;

public class PlayerFactory : IPlayerFactory
{
    public Result CreatePlayer()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        return Result.Success(player);
    }
}
