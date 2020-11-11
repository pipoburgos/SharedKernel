using System;
using SharedKernel.Domain.Exceptions;

namespace SharedKernel.Application.Exceptions
{
    [Serializable]
    public class SharedKernelApplicationException : ResourceException
    {
        private const string ResourcePath = "SharedKernel.Application.Exceptions.ExceptionCodes";

        public SharedKernelApplicationException(string code) : base(code, ResourcePath, typeof(SharedKernelApplicationException).Assembly) { }

        public SharedKernelApplicationException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelApplicationException).Assembly, innerException) { }
    }
}
