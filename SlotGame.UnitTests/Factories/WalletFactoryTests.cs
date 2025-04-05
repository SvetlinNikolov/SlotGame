namespace SlotGame.UnitTests.Factories;

using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Factories;

public class WalletFactoryTests
{
    private readonly WalletFactory _factory = new();

    [Fact]
    public void CreateWallet_ShouldSucceed_WhenFirstTimeForPlayer()
    {
        // Arrange
        var playerId = Guid.NewGuid();

        // Act
        var result = _factory.CreateWallet(playerId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.GetData<Wallet>());
        Assert.Equal(playerId, result.GetData<Wallet>().PlayerId);
    }

    [Fact]
    public void CreateWallet_ShouldFail_WhenWalletAlreadyExistsForPlayer()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        _factory.CreateWallet(playerId); // first time

        // Act
        var result = _factory.CreateWallet(playerId); // duplicate

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SlotGameErrors.DuplicateWallet(playerId).Message, result.Error?.Message);
    }
}
