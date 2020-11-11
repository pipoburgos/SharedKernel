using System;
using System.Security.Cryptography;
using System.Text;
using SharedKernel.Application.Security.Cryptography;

namespace SharedKernel.Infrastructure.Security.Cryptography
{
    public class EncryptionHexHelper : IEncryptionHexHelper
    {
        #region Properties

        //Algorithmo TripleDES
        private static readonly TripleDESCryptoServiceProvider Des = new TripleDESCryptoServiceProvider();

        //Objeto md5
        private static readonly MD5CryptoServiceProvider Hashmd5 = new MD5CryptoServiceProvider();

        //Clave secreta
        private const string MyKey = "TemplateWebApiSecretKey";

        #endregion Properties

        #region Private Functions

        /// <summary>
        ///     Funcion para coger los bytes de una cadena de texto en hex
        ///     <param name="hex">Texto con hex para pasar a bytes</param>
        ///     <returns>Array de bytes con el valor de la cadena hexadecimal</returns>
        /// </summary>
        private byte[] StringToByteArrayFastest(string hex)
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
        ///     Funcion para el coger el valor entero de un caracter hex
        ///     <param name="hex">Caracter hexadecimal para pasar a int</param>
        ///     <returns>Int con el valor del caracter</returns>
        /// </summary>
        private int GetHexVal(char hex)
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
        ///     Funcion para el Encriptado de Cadenas de Texto
        ///     <param name="texto">Texto a encriptar</param>
        ///     <returns>Cadena encriptada</returns>
        /// </summary>
        public string Encrypt(string texto)
        {
            string functionReturnValue = null;
            if (string.IsNullOrEmpty(texto.Trim()))
            {
                functionReturnValue = "";
            }
            else
            {
                Des.Key = Hashmd5.ComputeHash(new UnicodeEncoding().GetBytes(MyKey));
                Des.Mode = CipherMode.ECB;
                var encrypt = Des.CreateEncryptor();

                var buff = Encoding.UTF8.GetBytes(texto);
                buff = encrypt.TransformFinalBlock(buff, 0, buff.Length);
                //Convertimos los bytes a string de hex
                for (var i = 0; i < buff.Length; i++)
                    functionReturnValue += buff[i].ToString("X2");
            }
            return functionReturnValue;
        }

        /// <summary>
        ///     Funcion para el Desencriptado de Cadenas de Texto
        ///     <param name="texto">Texto a desencriptar</param>
        ///     <returns>Cadena desencriptada</returns>
        /// </summary>
        public string Decrypt(string texto)
        {
            string functionReturnValue;
            if (string.IsNullOrEmpty(texto.Trim()))
            {
                functionReturnValue = "";
            }
            else
            {
                Des.Key = Hashmd5.ComputeHash(new UnicodeEncoding().GetBytes(MyKey));
                Des.Mode = CipherMode.ECB;
                var desencrypta = Des.CreateDecryptor();

                //Convertimos el string de hex a bytes
                var buff = StringToByteArrayFastest(texto);
                buff = desencrypta.TransformFinalBlock(buff, 0, buff.Length);
                functionReturnValue = Encoding.UTF8.GetString(buff);
            }
            return functionReturnValue;
        }

        #endregion Funciones
    }
}
