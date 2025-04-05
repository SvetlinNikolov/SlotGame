using SlotGame.Enums;

namespace SlotGame.Helpers;

public static class ActionHelper
{
    public static (GameAction Action, decimal? Arg)? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (GameAction.Unknown, null);

        var parts = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        var commandString = parts.ElementAtOrDefault(0);
        var argString = parts.ElementAtOrDefault(1);

        var arg = decimal.TryParse(argString, out var parsedArg) ? parsedArg : (decimal?)null;

        Enum.TryParse<GameAction>(commandString, ignoreCase: true, out var action);

        return (action, arg);
    }
}
