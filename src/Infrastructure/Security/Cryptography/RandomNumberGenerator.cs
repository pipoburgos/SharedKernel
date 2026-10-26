

using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography;


/// <summary>
/// 
/// </summary>
public class RandomNumberGenerator : IRandomNumberGenerator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public int GetRandom(int minValue, int maxValue)
    {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        return global::System.Security.Cryptography.RandomNumberGenerator.GetInt32(minValue, maxValue);
#else
#pragma warning disable S2245
        var random = new Random();

        return random.Next(minValue, maxValue);
#pragma warning restore S2245
#endif
    }
}
