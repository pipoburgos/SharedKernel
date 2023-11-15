namespace SharedKernel.Domain.Exceptions;

/// <summary>
/// 
/// </summary>
[Serializable]
public abstract class SharedKernelDomainException : ResourceException
{
    private const string ResourcePath =  "SharedKernel.Domain.Exceptions.ExceptionCodes";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    protected SharedKernelDomainException(string code) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="innerException"></param>
    protected SharedKernelDomainException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly, innerException) { }
}