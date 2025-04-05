namespace SlotGame.UnitTests.Helpers;

using SlotGame.Enums;
using SlotGame.Helpers;

public class ActionHelperTests
{
    [Theory]
    [InlineData("deposit 100", GameAction.Deposit, 100)]
    [InlineData("withdraw 50", GameAction.Withdraw, 50)]
    [InlineData("bet 25", GameAction.Bet, 25)] // even though we don't allow bets above 10 we should still parse it
    [InlineData("BeT 10", GameAction.Bet, 10)]
    public void Parse_ShouldReturnCorrectActionAndArg_WhenInputIsValid(string input, GameAction expectedAction, int expectedArg)
    {
        // Act
        var result = ActionHelper.Parse(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedAction, result?.Action);
        Assert.Equal((decimal)expectedArg, result?.Arg);
    }

    [Theory]
    [InlineData("exit")]
    [InlineData("Exit")]
    public void Parse_ShouldReturnActionWithNullArg_WhenNoArgumentProvided(string input)
    {
        // Act
        var result = ActionHelper.Parse(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(GameAction.Exit, result?.Action);
        Assert.Null(result?.Arg);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Parse_ShouldReturnUnknown_WhenInputIsNullOrEmpty(string? input)
    {
        // Act
        var result = ActionHelper.Parse(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(GameAction.Unknown, result?.Action);
        Assert.Null(result?.Arg);
    }

    [Fact]
    public void Parse_ShouldReturnUnknown_WhenCommandIsInvalid()
    {
        var result = ActionHelper.Parse("fly 999");

        Assert.NotNull(result);
        Assert.Equal(GameAction.Unknown, result?.Action);
        Assert.Equal(999m, result?.Arg);
    }

    [Fact]
    public void Parse_ShouldReturnNullArg_WhenArgIsNotANumber()
    {
        var result = ActionHelper.Parse("deposit abc");

        Assert.NotNull(result);
        Assert.Equal(GameAction.Deposit, result?.Action);
        Assert.Null(result?.Arg);
    }
}
