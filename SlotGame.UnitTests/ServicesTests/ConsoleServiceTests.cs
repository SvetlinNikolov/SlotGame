using SlotGame.Domain.Errors;
using SlotGame.Services;

namespace SlotGame.UnitTests.ServicesTests;

public class ConsoleServiceTests
{
    [Fact]
    public void PrintError_String_ShouldWrite_ErrorWithPrefixAndColor()
    {
        // Arrange
        var consoleService = new ConsoleService();
        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        consoleService.PrintError("something went wrong");

        // Assert
        var output = sw.ToString().Trim();
        Assert.Equal("Error: something went wrong", output);
    }

    [Fact]
    public void PrintError_ErrorObject_ShouldWrite_ErrorMessage()
    {
        // Arrange
        var error = new Error("Something exploded.");
        var consoleService = new ConsoleService();
        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        consoleService.PrintError(error);

        // Assert
        var output = sw.ToString().Trim();
        Assert.Equal("Error: Something exploded.", output);
    }

    [Fact]
    public void PrintInfo_ShouldWrite_PlainText()
    {
        // Arrange
        var consoleService = new ConsoleService();
        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        consoleService.PrintInfo("Balance updated.");

        // Assert
        var output = sw.ToString().Trim();
        Assert.Equal("Balance updated.", output);
    }

    [Fact]
    public void ReadLine_ShouldReturn_UserInput()
    {
        // Arrange
        var input = "deposit 50";
        using var sr = new StringReader(input);
        Console.SetIn(sr);
        var consoleService = new ConsoleService();

        // Act
        var result = consoleService.ReadLine();

        // Assert
        Assert.Equal("deposit 50", result);
    }
}
