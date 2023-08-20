using SharedKernel.Application.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Polly.Requests.Middlewares;

/// <summary> RetryPolicyMiddleware Exception handler. </summary>
public class RetryPolicyExceptionHandler : IRetryPolicyExceptionHandler
{
    /// <summary> Check if an exception has to be retried. </summary>
    public bool NeedToRetryTheException(Exception exception)
    {
        return true;
    }
}
