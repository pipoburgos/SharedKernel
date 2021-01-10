using System;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;

namespace SharedKernel.Domain.Exceptions
{
    /// <summary>
    /// Resource exception
    /// </summary>
    [Serializable]
    public abstract class ResourceException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="resourcePath"></param>
        /// <param name="assembly"></param>
        protected ResourceException(string code, string resourcePath, Assembly assembly) : base(new ResourceManager(resourcePath, assembly).GetString(code))
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="resourcePath"></param>
        /// <param name="assembly"></param>
        /// <param name="innerException"></param>
        protected ResourceException(string code, string resourcePath, Assembly assembly, Exception innerException) : base(new ResourceManager(resourcePath, assembly).GetString(code), innerException)
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected ResourceException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
