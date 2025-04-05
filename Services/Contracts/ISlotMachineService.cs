using SlotGame.Domain.Models;
using SlotGame.Domain.Result;

namespace SlotGame.Services.Contracts;

public interface ISlotMachineService
{
    Result Spin(Wallet wallet, decimal betAmount);
}
