using SlotGame.Domain.Errors;

namespace SlotGame.Services.Contracts;

public interface IConsoleService
{
    void PrintInfo(string? message = null);
    void PrintError(string message);
    void PrintError(Error error);
    string? ReadLine();
}
