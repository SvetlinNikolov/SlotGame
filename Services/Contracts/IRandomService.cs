using System.Security.Cryptography;

namespace SlotGame.Services.Contracts;

public interface IRandomService
{
    public int GetRandomInt(int min, int max);
}
