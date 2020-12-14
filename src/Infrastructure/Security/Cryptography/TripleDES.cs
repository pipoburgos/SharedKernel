using System.Security.Cryptography;
using System.Text;
using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    public class TripleDes : ITripleDes
    {
        public string Encrypt(string textKey, string content)
        {
            var key = Encoding.GetEncoding(1252).GetBytes(textKey);
            var iv = new byte[8];
            var data = Encoding.GetEncoding(1252).GetBytes(content);
            var tripleDes = TripleDES.Create();
            tripleDes.IV = iv;
            tripleDes.Key = key;
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.Zeros;
            var ict = tripleDes.CreateEncryptor();
            var enc = ict.TransformFinalBlock(data, 0, data.Length);
            return Encoding.GetEncoding(1252).GetString(enc);
        }
    }
}
