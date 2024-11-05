//using System.Text;

//namespace SharedKernel.Infrastructure.RedsysTPV.Helpers;

///// <summary> . </summary>
//public static class Base64
//{
//    /// <summary> . </summary>
//    public static string EncodeTo64(string data)
//    {
//        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//        var toEncodeAsBytes = Encoding.GetEncoding(1252).GetBytes(data);
//        return Convert.ToBase64String(toEncodeAsBytes);
//    }

//    /// <summary> . </summary>
//    public static string DecodeFrom64(string data)
//    {
//        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//        var binary = Convert.FromBase64String(data);
//        return Encoding.GetEncoding(1252).GetString(binary);
//    }
//}