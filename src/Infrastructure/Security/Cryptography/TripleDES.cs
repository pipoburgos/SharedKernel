using SharedKernel.Application.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
#pragma warning disable S5547

namespace SharedKernel.Infrastructure.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public class TripleDes : ITripleDes
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="textKey"></param>
    /// <param name="content"></param>
    /// <returns></returns>
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
        var ict = tripleDes.CreateEncryptor(tripleDes.Key, tripleDes.IV);
        var enc = ict.TransformFinalBlock(data, 0, data.Length);
        return Encoding.GetEncoding(1252).GetString(enc);
    }
}