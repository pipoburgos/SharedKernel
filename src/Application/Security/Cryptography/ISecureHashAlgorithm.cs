namespace SharedKernel.Application.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public interface ISecureHashAlgorithm
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    string Generate512String(string inputString);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    string Generate512String(byte[] bytes);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    string Generate256String(string inputString);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    string Generate256String(byte[] bytes);
}