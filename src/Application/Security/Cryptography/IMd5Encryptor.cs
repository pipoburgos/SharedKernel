namespace SharedKernel.Application.Security.Cryptography;

/// <summary>
/// Encription helper
/// </summary>
public interface IMd5Encryptor
{
    /// <summary>
    ///     Function for Text String Encryption
    ///     <param name="text">Text to encrypt</param>
    ///     <param name="secretKey">Secret key</param>
    ///     <returns>Encrypted string</returns>
    /// </summary>
    string Encrypt(string text, string secretKey);

    /// <summary>
    ///     Function for decrypting text strings
    ///     <param name="text">Text to decrypt</param>
    ///     <param name="secretKey">Secret key</param>
    ///     <returns>Decrypted string</returns>
    /// </summary>
    string Decrypt(string text, string secretKey);
}