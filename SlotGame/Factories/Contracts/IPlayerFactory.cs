using SlotGame.Domain.Models;
using SlotGame.Domain.Result;

namespace SlotGame.Factories.Contracts;

public interface IPlayerFactory
{
    Player CreatePlayer();
}
