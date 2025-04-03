namespace SlotGame.Factories.Contracts;

public interface IPlayerFactory
{
    Task<Player> CreatePlayerAsync();
}
