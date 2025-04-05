using SlotGame.Services.Contracts;
using System.Security.Cryptography;

namespace SlotGame.Services;

public class RandomService : IRandomService
{
    public decimal GetRandomDecimal(decimal min, decimal max, int precision = 2)
    {
        int scale = (int)Math.Pow(10, precision);
        int rangeMin = (int)(min * scale);
        int rangeMax = (int)(max * scale);

        int random = RandomNumberGenerator.GetInt32(rangeMin, rangeMax + 1);
        return random / (decimal)scale;
    }

    public int GetRandomInt(int min, int max)
    {
        return RandomNumberGenerator.GetInt32(min, max + 1);
    }
}
