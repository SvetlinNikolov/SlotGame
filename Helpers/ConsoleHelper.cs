using SlotGame.Domain.Errors;

namespace SlotGame.Helpers;

public static class ConsoleHelper
{
    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    public static void PrintError(Error error)
    {
        PrintError(error.Message);
    }

    public static void PrintInfo(string message)
    {
        Console.WriteLine(message);
    }
}
