using System;
using SharedKernel.Domain.Exceptions;

namespace SharedKernel.Application.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SharedKernelApplicationException : ResourceException
    {
        private const string ResourcePath = "SharedKernel.Application.Exceptions.ExceptionCodes";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public SharedKernelApplicationException(string code) : base(code, ResourcePath, typeof(SharedKernelApplicationException).Assembly) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        public SharedKernelApplicationException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelApplicationException).Assembly, innerException) { }
    }
}
