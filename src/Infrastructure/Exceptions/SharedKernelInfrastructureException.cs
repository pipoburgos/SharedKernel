using System;
using SharedKernel.Domain.Exceptions;

namespace SharedKernel.Infrastructure.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SharedKernelInfrastructureException : ResourceException
    {
        private const string ResourcePath = "SharedKernel.Infrastructure.Exceptions.ExceptionCodes";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public SharedKernelInfrastructureException(string code) : base(code, ResourcePath, typeof(SharedKernelInfrastructureException).Assembly) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        public SharedKernelInfrastructureException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelInfrastructureException).Assembly, innerException) { }
    }
}
