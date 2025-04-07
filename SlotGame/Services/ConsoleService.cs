using SlotGame.Domain.Errors;
using SlotGame.Services.Contracts;

namespace SlotGame.Services;

public class ConsoleService : IConsoleService
{
    public void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    public void PrintError(Error error)
    {
        PrintError(error.Message);
    }

    public void PrintInfo(string? message = null)
    {
        Console.WriteLine(message);
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
