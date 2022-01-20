using System;

namespace SharedKernel.Application.Cqrs.Middlewares
{
    /// <summary>
    /// RetryPolicyMiddleware Exception handler
    /// </summary>
    public interface IRetryPolicyExceptionHandler
    {
        /// <summary>
        /// Check if an exception has to be retried
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool NeedToRetryTheException(Exception exception);
    }
}
