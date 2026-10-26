namespace SharedKernel.Application.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public interface ISha256
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    string HashHmac(string data, string key);

    /// <summary>
    /// Get HMAC SHA256 signature with 3DES key
    /// </summary>
    /// <param name="tripeDesKey"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    string GetSignature(byte[] tripeDesKey, string value);

    /// <summary>
    /// Computes a hash value for the specified byte array. 
    /// </summary>
    /// <param name="bytes">The input data to hash.</param>
    /// <returns>A string representation of the computed hash.</returns>
    string ComputeHash(byte[] bytes);

    /// <summary>
    /// Computes a hash value for the specified byte array. 
    /// </summary>
    /// <param name="stream">The input data to hash.</param>
    /// <returns>A string representation of the computed hash.</returns>
    string ComputeHash(Stream stream);
}