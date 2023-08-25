using SharedKernel.Application.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public class Md5Encryptor : IMd5Encryptor
    {
        #region Properties

        // TripleDES Algorithm
        private static readonly TripleDES Des = TripleDES.Create();

        // Objet md5
        private static readonly MD5 HashMd5 = MD5.Create();
        //private static readonly HMACMD5 HashMd5 = new();

        #endregion Properties

        #region Private Functions

        /// <summary>
        ///     Function to get the bytes of a text string in hex
        ///     <param name="hex">Text with hex to pass to bytes</param>
        ///     <returns>Byte array with the hexadecimal string value</returns>
        /// </summary>
        private static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            var arr = new byte[hex.Length >> 1];

            var to = hex.Length >> 1;
            for (var i = 0; i < to; ++i)
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));

            return arr;
        }

        /// <summary>
        ///     Function to get the integer value of a hex character
        ///     <param name="hex">Hexadecimal character to pass to int</param>
        ///     <returns>Int with the character value</returns>
        /// </summary>
        private static int GetHexVal(char hex)
        {
            var val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
        }

        #endregion

        #region Funciones

        /// <summary>
        ///     Function for Text String Encryption
        ///     <param name="text">Text to encrypt</param>
        ///     <param name="secretKey">Text to encrypt</param>
        ///     <returns>Encrypted string</returns>
        /// </summary>
        public string Encrypt(string text, string secretKey)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                return string.Empty;

            Des.Key = HashMd5.ComputeHash(new UnicodeEncoding().GetBytes(secretKey));
            Des.Mode = CipherMode.ECB;
            var encrypt = Des.CreateEncryptor();

            var buff = Encoding.UTF8.GetBytes(text);
            buff = encrypt.TransformFinalBlock(buff, 0, buff.Length);
            // Convert bytes to string from hex
            var functionReturnValue = string.Empty;
            for (var i = 0; i < buff.Length; i++)
                functionReturnValue += buff[i].ToString("X2");

            return functionReturnValue;
        }

        /// <summary>
        ///     Function for Decrypting Text Strings
        ///     <param name="text">Text to decrypt</param>
        ///     <param name="secretKey">Text to encrypt</param>
        ///     <returns>Decrypted string</returns>
        /// </summary>
        public string Decrypt(string text, string secretKey)
        {
            string functionReturnValue;
            if (string.IsNullOrEmpty(text.Trim()))
            {
                functionReturnValue = "";
            }
            else
            {
                Des.Key = HashMd5.ComputeHash(new UnicodeEncoding().GetBytes(secretKey));
                Des.Mode = CipherMode.ECB;
                var decryptor = Des.CreateDecryptor();
                var buff = StringToByteArrayFastest(text);
                buff = decryptor.TransformFinalBlock(buff, 0, buff.Length);
                functionReturnValue = Encoding.UTF8.GetString(buff);
            }
            return functionReturnValue;
        }

        #endregion Funciones
    }
}
