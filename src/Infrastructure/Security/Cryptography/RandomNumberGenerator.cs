using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
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
            var rng = global::System.Security.Cryptography.RandomNumberGenerator.Create();
            var rndBytes = new byte[4];
            rng.GetBytes(rndBytes);
            var rand = BitConverter.ToInt32(rndBytes, 0);

            var gen = new Random(rand);
            return gen.Next(minValue, maxValue);
        }
    }
}
