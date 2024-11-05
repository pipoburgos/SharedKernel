//using System.Security.Cryptography;
//using System.Text;

//// ReSharper disable InconsistentNaming

//namespace SharedKernel.Infrastructure.RedsysTPV.Helpers;

///// <summary> . </summary>
//public static class TripleDES
//{
//    /// <summary> . </summary>
//    public static string Encrypt(string textKey, string content)
//    {
//        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//        var key = Encoding.GetEncoding(1252).GetBytes(textKey);
//        var iv = new byte[8];
//        var data = Encoding.GetEncoding(1252).GetBytes(content);
//        var tdes = System.Security.Cryptography.TripleDES.Create();
//        tdes.IV = iv;
//        tdes.Key = key;
//        tdes.Mode = CipherMode.CBC;
//        tdes.Padding = PaddingMode.Zeros;
//        var ict = tdes.CreateEncryptor();
//        var enc = ict.TransformFinalBlock(data, 0, data.Length);
//        return Encoding.GetEncoding(1252).GetString(enc);
//    }
//}