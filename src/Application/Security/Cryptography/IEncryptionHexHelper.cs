namespace SharedKernel.Application.Security.Cryptography
{
    public interface IEncryptionHexHelper
    {
        /// <summary>
        ///     Function for Text String Encryption
        ///     <param name="text">Text to encrypt</param>
        ///     <returns>Encrypted string</returns>
        /// </summary>
        string Encrypt(string text);

        /// <summary>
        ///     Function for decrypting text strings
        ///     <param name="text">Text to decrypt</param>
        ///     <returns>Decrypted string</returns>
        /// </summary>
        string Decrypt(string text);
    }
}
