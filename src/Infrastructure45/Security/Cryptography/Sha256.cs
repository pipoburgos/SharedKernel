using System;
using System.Security.Cryptography;
using System.Text;
using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    public class Sha256 : ISha256
    {
#if NETSTANDARD
        public Sha256()
        {
            // Fix: NotSupportedException: No data is available for encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif

        public string HashHmac(string data, string key)
        {
            key = key ?? "";
            var encoding = Encoding.GetEncoding(1252);
            var keyByte = encoding.GetBytes(key);
            var messageBytes = encoding.GetBytes(data);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// Get HMAC SHA256 signature with 3DES key
        /// </summary>
        /// <param name="tripeDesKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetSignature(byte[] tripeDesKey, string value)
        {
            string signature;
            using (var hmac = new HMACSHA256(tripeDesKey))
            {
                signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
            }

            return signature;
        }
    }
}
