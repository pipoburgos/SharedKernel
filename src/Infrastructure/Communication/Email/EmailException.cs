using System;
using SharedKernel.Infrastructure.Exceptions;

namespace SharedKernel.Infrastructure.Communication.Email
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EmailException : SharedKernelInfrastructureException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public EmailException(string code) : base(code) { }
    }
}
