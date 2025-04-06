namespace SlotGame.Services;

using SlotGame.Services.Contracts;

public class ApplicationControl : IApplicationControl
{
    public void Exit(int code = 0)
    {
        Environment.Exit(code);
    }
}
