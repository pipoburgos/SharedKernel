//using System.Security.Cryptography;
//using System.Text;

//// ReSharper disable InconsistentNaming

//namespace SharedKernel.Infrastructure.RedsysTPV.Helpers;

///// <summary> . </summary>
//public static class SHA256
//{
//    /// <summary> . </summary>
//    public static string HashHMAC(string data, string? key)
//    {
//        key ??= "";
//        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//        var encoding = Encoding.GetEncoding(1252);
//        var keyByte = encoding.GetBytes(key);
//        var messageBytes = encoding.GetBytes(data);
//        using (var hmacsha256 = new HMACSHA256(keyByte))
//        {
//            var hashmessage = hmacsha256.ComputeHash(messageBytes);
//            return Convert.ToBase64String(hashmessage);
//        }
//    }
//}