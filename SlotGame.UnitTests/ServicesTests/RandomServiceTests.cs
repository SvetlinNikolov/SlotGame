using SlotGame.Services;

namespace SlotGame.UnitTests.ServicesTests;

public class RandomServiceTests
{
    private readonly RandomService _randomService = new();

    [Theory]
    [InlineData(1, 100)]
    [InlineData(0.5, 1.5)]
    [InlineData(100, 200)]
    public void GetRandomDecimal_ShouldReturnValueWithinRange(decimal min, decimal max)
    {
        //Act & Assert

        for (int i = 0; i < 1000; i++)
        {
            var result = _randomService.GetRandomDecimal(min, max);
            Assert.InRange(result, min, max);
        }
    }

    [Fact]
    public void GetRandomDecimal_MinGreaterThanMax_ThrowsException()
    {
        //Act & Assert

        Assert.Throws<ArgumentException>(() =>
            _randomService.GetRandomDecimal(10, 5));
    }
}
