namespace SlotGame.Services.Contracts;

public interface IRandomService
{
    /// <summary>
    /// <summary>
    /// Generates a random integer within the specified inclusive range.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number to be generated.</param>
    /// <param name="max">The inclusive upper bound of the random number to be generated. Must be greater than or equal to <paramref name="min"/>.</param>
    /// <returns>A random integer greater than or equal to <paramref name="min"/> and less than or equal to <paramref name="max"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    int GetRandomInt(int min, int max);

    /// <summary>
    /// Generates a random decimal number within the specified inclusive range, rounded to the given precision.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number to be generated.</param>
    /// <param name="max">The inclusive upper bound of the random number to be generated. Must be greater than or equal to <paramref name="min"/>.</param>
    /// <param name="precision">The number of decimal places to round the result to. Defaults to 2.</param>
    /// <returns>A random decimal number greater than or equal to <paramref name="min"/> and less than or equal to <paramref name="max"/>, rounded to the specified precision.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than or equal to <paramref name="max"/>.</exception>
    decimal GetRandomDecimal(decimal min, decimal max, int precision = 2);
}
