using System.Text;
using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public class Base64 : IBase64
    {
#if NETSTANDARD
        /// <summary>
        /// 
        /// </summary>
        public Base64()
        {
            // Fix: NotSupportedException: No data is available for encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EncodeTo64(string data)
        {
            var toEncodeAsBytes = Encoding.GetEncoding(1252).GetBytes(data);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DecodeFrom64(string data)
        {
            var binary = Convert.FromBase64String(data);
            return Encoding.GetEncoding(1252).GetString(binary);
        }
    }
}
