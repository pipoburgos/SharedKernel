using SharedKernel.Application.Exceptions;

namespace SharedKernel.Application.Communication.Email;

/// <summary>
/// 
/// </summary>
[Serializable]
public class EmailException : SharedKernelApplicationException
{
    /// <summary>  </summary>
    public EmailException(string code) : base(code) { }

    /// <summary>  </summary>
    public EmailException(Exception ex) : base("exc", ex) { }
}
