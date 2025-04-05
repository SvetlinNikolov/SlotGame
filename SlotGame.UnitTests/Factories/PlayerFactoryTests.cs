namespace SlotGame.UnitTests.Factories;

using SlotGame.Factories;

public class PlayerFactoryTests
{
    private readonly PlayerFactory _factory = new();

    [Fact]
    public void CreatePlayer_ShouldReturnPlayerWithValidId()
    {
        // Act
        var player = _factory.CreatePlayer();

        // Assert
        Assert.NotNull(player);
        Assert.NotEqual(Guid.Empty, player.Id);
    }

    [Fact]
    public void CreatePlayer_ShouldReturnNewInstanceEachTime()
    {
        // Act
        var player1 = _factory.CreatePlayer();
        var player2 = _factory.CreatePlayer();

        // Assert
        Assert.NotEqual(player1.Id, player2.Id);
    }
}
