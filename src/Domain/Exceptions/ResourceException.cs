using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;

namespace SharedKernel.Domain.Exceptions;

/// <summary>
/// Resource exception
/// </summary>
[Serializable]
public abstract class ResourceException : Exception
{
    /// <summary> Gets the Code. </summary>
    public string Code { get; }

    /// <summary> Instanciates a new instance of the <see cref="ResourceException"/> class with a specified error code and an inner exception. </summary>
    protected ResourceException(string code, string resourcePath, Assembly assembly) : base(new ResourceManager(resourcePath, assembly).GetString(code))
    {
        Code = code;
    }

    /// <summary> Instanciates a new instance of the <see cref="ResourceException"/> class with a specified error code and an inner exception. </summary>
    protected ResourceException(string code, string resourcePath, Assembly assembly, Exception innerException) : base(new ResourceManager(resourcePath, assembly).GetString(code), innerException)
    {
        Code = code;
    }

    /// <summary> Instanciates a new instance of the <see cref="ResourceException"/> class with a specified error code and an inner exception. </summary>
    protected ResourceException() { }

    /// <summary> Instanciates a new instance of the <see cref="ResourceException"/> class with a specified error code and an inner exception. </summary>
    protected ResourceException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}