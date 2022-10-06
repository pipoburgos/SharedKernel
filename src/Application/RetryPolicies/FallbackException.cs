using System;

namespace SharedKernel.Application.RetryPolicies
{
    /// <summary> Retries fails. </summary>
    public class FallbackException : Exception
    {
        /// <summary> Constructor. </summary>
        /// <param name="innerException"></param>
        public FallbackException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}
