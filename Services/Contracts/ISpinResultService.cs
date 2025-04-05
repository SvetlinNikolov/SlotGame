using SlotGame.Domain.Result;

namespace SlotGame.Services.Contracts;

public interface ISpinResultService
{
    Result GetSpinResult(decimal betAmount);
}
