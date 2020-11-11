namespace SharedKernel.Application.Security.Cryptography
{
    public interface ISha256
    {
        string HashHmac(string data, string key);

        /// <summary>
        /// Get HMAC SHA256 signature with 3DES key
        /// </summary>
        /// <param name="tripeDesKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetSignature(byte[] tripeDesKey, string value);
    }
}
