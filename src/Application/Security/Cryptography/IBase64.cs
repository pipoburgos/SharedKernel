namespace SharedKernel.Application.Security.Cryptography
{
    /// <summary>
    /// Base 64 helper
    /// </summary>
    public interface IBase64
    {
        /// <summary>
        /// Encode to base 64 string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string EncodeTo64(string data);

        /// <summary>
        /// Decode from base 64 string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string DecodeFrom64(string data);
    }
}