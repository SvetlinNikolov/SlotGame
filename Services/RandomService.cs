using SlotGame.Services.Contracts;
using System.Security.Cryptography;

namespace SlotGame.Services;

public class RandomService : IRandomService
{
    /// <summary>
    /// Returns a cryptographically random integer between min (inclusive) and max (exclusive).
    /// </summary>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <returns>A random integer in the specified range.</returns>
    /// 
    public int GetRandomInt(int min, int max)
    {
        return RandomNumberGenerator.GetInt32(min, max);
    }
}
