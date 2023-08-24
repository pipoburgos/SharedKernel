using SharedKernel.Infrastructure.Exceptions;

namespace SharedKernel.Infrastructure.Communication.Email
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EmailException : SharedKernelInfrastructureException
    {
        /// <summary>  </summary>
        public EmailException(string code) : base(code) { }

        /// <summary>  </summary>
        public EmailException(Exception ex) : base("exc", ex) { }
    }
}
