using System.Runtime.Serialization;

namespace SharedKernel.Domain.Exceptions;

/// <summary>
/// 
/// </summary>
[Serializable]
public abstract class SharedKernelDomainException : ResourceException
{
    private const string ResourcePath = "SharedKernel.Domain.Exceptions.ExceptionCodes";

    /// <summary> Instanciates a new instance of the <see cref="SharedKernelDomainException"/> class with a specified error code and an inner exception. </summary>
    protected SharedKernelDomainException(string code) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly) { }

    /// <summary> Instanciates a new instance of the <see cref="SharedKernelDomainException"/> class with a specified error code and an inner exception. </summary>
    protected SharedKernelDomainException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly, innerException) { }

    /// <summary> Instanciates a new instance of the <see cref="SharedKernelDomainException"/> class with a specified error code and an inner exception. </summary>
    protected SharedKernelDomainException() { }

    /// <summary> Instanciates a new instance of the <see cref="SharedKernelDomainException"/> class with a specified error code and an inner exception. </summary>
    protected SharedKernelDomainException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}