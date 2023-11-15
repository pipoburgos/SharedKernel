namespace SharedKernel.Application.Security.Cryptography;

/// <summary>
/// Random number generator
/// </summary>
public interface IRandomNumberGenerator
{
    /// <summary>
    /// Generates a random number
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    int GetRandom(int minValue, int maxValue);
}