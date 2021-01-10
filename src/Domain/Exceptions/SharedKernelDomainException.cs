using System;
using System.Runtime.Serialization;

namespace SharedKernel.Domain.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SharedKernelDomainException : ResourceException
    {
        [NonSerialized]
        private const string ResourcePath = "SharedKernel.Domain.Exceptions.ExceptionCodes";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public SharedKernelDomainException(string code) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        public SharedKernelDomainException(string code, Exception innerException) : base(code, ResourcePath, typeof(SharedKernelDomainException).Assembly, innerException) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public SharedKernelDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
