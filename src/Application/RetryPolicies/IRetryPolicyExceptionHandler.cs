namespace SharedKernel.Application.RetryPolicies;

/// <summary> RetryPolicyMiddleware Exception handler. </summary>
public interface IRetryPolicyExceptionHandler
{
    /// <summary> Check if an exception has to be retried. </summary>
    bool NeedToRetryTheException(Exception exception);
}
