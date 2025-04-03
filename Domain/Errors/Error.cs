namespace SlotGame.Domain.Errors;

public class Error : Exception
{
    public new string Message { get; }

    public Error(string message)
    {
        Message = message;
    }

    public override string ToString() => $"{Message}";
}
