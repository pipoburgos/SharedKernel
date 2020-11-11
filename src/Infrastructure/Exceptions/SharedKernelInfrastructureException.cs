using System;
using SharedKernel.Domain.Exceptions;

namespace SharedKernel.Infrastructure.Exceptions
{
    [Serializable]
    public class SharedKernelInfrastructureException : ResourceException
    {
        private const string ResourcePath = "SharedKernel.Infrastructure.Exceptions.ExceptionCodes";

        public SharedKernelInfrastructureException(string code) : base(code, ResourcePath, typeof(SharedKernelInfrastructureException).Assembly) { }

        public SharedKernelInfrastructureException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelInfrastructureException).Assembly, innerException) { }
    }
}
