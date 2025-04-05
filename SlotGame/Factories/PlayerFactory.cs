using SlotGame.Domain.Models;
using SlotGame.Factories.Contracts;

namespace SlotGame.Factories;

public class PlayerFactory : IPlayerFactory
{
    public Player CreatePlayer()
    {
        var player = new Player
        {
            Id = Guid.NewGuid()
        };

        return player;
    }
}
