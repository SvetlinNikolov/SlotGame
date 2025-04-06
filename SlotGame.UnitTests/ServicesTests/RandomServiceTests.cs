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
        for (int i = 0; i < 1000; i++)
        {
            var result = _randomService.GetRandomDecimal(min, max);
            Assert.InRange(result, min, max);
        }
    }

    [Fact]
    public void GetRandomDecimal_MinGreaterThanOMax_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _randomService.GetRandomDecimal(10, 5));
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(0, 100)]
    [InlineData(50, 51)]
    public void GetRandomInt_ShouldReturnValueWithinInclusiveRange(int min, int max)
    {
        for (int i = 0; i < 1000; i++)
        {
            var result = _randomService.GetRandomInt(min, max);
            Assert.InRange(result, min, max);
        }
    }

    [Fact]
    public void GetRandomInt_MinGreaterThanMax_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _randomService.GetRandomInt(10, 5));
    }
}
