using System;

namespace SharedKernel.Domain.Exceptions
{
    [Serializable]
    public abstract class SharedKernelDomainException : ResourceException
    {
        private const string ResourcePath =  "SharedKernel.Domain.Exceptions.ExceptionCodes";

        protected SharedKernelDomainException(string code) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly) { }

        protected SharedKernelDomainException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly, innerException) { }
    }
}
