using SlotGame.Domain.Errors;

namespace SlotGame.Services.Contracts;

public interface IConsoleService
{
    void PrintInfo(string message);
    void PrintError(string message);
    void PrintError(Error error);
    string? ReadLine();
}
