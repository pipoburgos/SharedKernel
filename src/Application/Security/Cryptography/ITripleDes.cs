namespace SharedKernel.Application.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITripleDes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        string Encrypt(string textKey, string content);
    }
}
