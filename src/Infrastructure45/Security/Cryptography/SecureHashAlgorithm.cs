using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    public static class SecureHashAlgorithm
    {
        public static string Generate512String(string inputString)
        {
            var bytes = Encoding.UTF8.GetBytes(inputString);
            return Generate512String(bytes);
        }

        public static string Generate512String(byte[] bytes)
        {
            var hash = SHA512.Create().ComputeHash(bytes);
            return GetStringFromHash(hash);
        }


        public static string Generate256String(string inputString)
        {
            var bytes = Encoding.UTF8.GetBytes(inputString);
            return Generate256String(bytes);
        }

        public static string Generate256String(byte[] bytes)
        {
            var hash = SHA256.Create().ComputeHash(bytes);
            return GetStringFromHash(hash);
        }


        private static string GetStringFromHash(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (var character in hash)
                result.Append(character.ToString("X2"));

            return result.ToString();
        }
    }
}