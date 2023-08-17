using SharedKernel.Application.Cqrs.Middlewares;
using System;

namespace SharedKernel.Infrastructure.Requests.Middlewares.RetryPolicy;

/// <summary> RetryPolicyMiddleware Exception handler. </summary>
public class RetryPolicyExceptionHandler : IRetryPolicyExceptionHandler
{
    /// <summary> Check if an exception has to be retried. </summary>
    public bool NeedToRetryTheException(Exception exception)
    {
        return true;
    }
}
