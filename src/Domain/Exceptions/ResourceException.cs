using System;
using System.Reflection;
using System.Resources;

namespace SharedKernel.Domain.Exceptions
{
    [Serializable]
    public abstract class ResourceException : Exception
    {
        public string Code { get; }

        protected ResourceException(string code, string resourcePath, Assembly assembly) : base(new ResourceManager(resourcePath, assembly).GetString(code))
        {
            Code = code;
        }

        protected ResourceException(string code, string resourcePath, Assembly assembly, Exception innerException) : base(new ResourceManager(resourcePath, assembly).GetString(code), innerException)
        {
            Code = code;
        }

        //public DomainException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        //{
        //}
    }
}
