using System;
using System.Text;
using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    public class Base64 : IBase64
    {
#if NETSTANDARD
        public Base64()
        {
            // Fix: NotSupportedException: No data is available for encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif

        public string EncodeTo64(string data)
        {
            var toEncodeAsBytes = Encoding.GetEncoding(1252).GetBytes(data);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public string DecodeFrom64(string data)
        {
            var binary = Convert.FromBase64String(data);
            return Encoding.GetEncoding(1252).GetString(binary);
        }
    }
}
