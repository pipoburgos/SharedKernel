namespace SharedKernel.Application.System;

/// <summary>
/// 
/// </summary>
public interface IStreamHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    byte[] ToByteArray(Stream input);
}