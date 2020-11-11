using System;

namespace SharedKernel.Domain.Exceptions
{
    [Serializable]
    public abstract class DomainException : SharedKernelException
    {
        public static string ResourcePath =  "SharedKernel.Domain.Exceptions.ExceptionCodes";

        protected DomainException(string code) : base(code, ResourcePath, typeof(DomainException).Assembly) { }

        protected DomainException(string code, Exception innerException) : base(code, ResourcePath, typeof(DomainException).Assembly, innerException) { }
    }
}
