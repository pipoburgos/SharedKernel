namespace SharedKernel.Application.RetryPolicies;

/// <summary>
/// Retry retriever provides an ability to automatically re-invoke a failed operation
/// </summary>
public interface IRetriever
{
    /// <summary>  </summary>
    int RetryCount { get; }

    /// <summary>
    /// Retry retriever provides an ability to automatically re-invoke a failed operation
    /// </summary>
    Task ExecuteAsync<TResult>(Func<CancellationToken, Task> action,
        Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken);

    /// <summary>
    /// Retry retriever provides an ability to automatically re-invoke a failed operation
    /// </summary>
    Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action,
        Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken);
}
