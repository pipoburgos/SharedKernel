using SharedKernel.Application.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public class Sha256 : ISha256
    {
#if NETSTANDARD
        /// <summary>
        /// 
        /// </summary>
        public Sha256()
        {
            // Fix: NotSupportedException: No data is available for encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string HashHmac(string data, string? key)
        {
            key ??= string.Empty;
            var encoding = Encoding.GetEncoding(1252);
            var keyByte = encoding.GetBytes(key);
            var messageBytes = encoding.GetBytes(data);
            using var hmacSha256 = new HMACSHA256(keyByte);
            var hashMessage = hmacSha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashMessage);
        }

        /// <summary>
        /// Get HMAC SHA256 signature with 3DES key
        /// </summary>
        /// <param name="tripeDesKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetSignature(byte[] tripeDesKey, string value)
        {
            using var hmac = new HMACSHA256(tripeDesKey);
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));

            return signature;
        }
    }
}
