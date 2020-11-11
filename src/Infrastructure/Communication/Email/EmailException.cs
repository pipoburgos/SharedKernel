using System;
using SharedKernel.Infrastructure.Exceptions;

namespace SharedKernel.Infrastructure.Communication.Email
{
    [Serializable]
    public class EmailException : SharedKernelInfrastructureException
    {
        public EmailException(string code) : base(code) { }
    }
}
