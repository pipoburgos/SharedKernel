using System;
using System.Security.Cryptography;
using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public int GetRandom(int minValue, int maxValue)
        {
            var rng = new RNGCryptoServiceProvider();
            var rndBytes = new byte[4];
            rng.GetBytes(rndBytes);
            var rand = BitConverter.ToInt32(rndBytes, 0);

            var gen = new Random(rand);
            return gen.Next(minValue, maxValue);
        }
    }
}
