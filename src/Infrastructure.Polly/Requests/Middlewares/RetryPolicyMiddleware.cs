using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RailwayOrientedProgramming;
using SharedKernel.Application.Requests;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Domain.Requests;

namespace SharedKernel.Infrastructure.Polly.Requests.Middlewares;

/// <summary>
/// Retry retriever provides an ability to automatically re-invoke a failed operation.
/// </summary>
public class RetryPolicyMiddleware : IMiddleware
{
    private readonly IRetriever _retryRetriever;
    private readonly IRetryPolicyExceptionHandler _retryPolicyExceptionHandler;

    /// <summary> Constructor. </summary>
    public RetryPolicyMiddleware(
        IRetriever retryRetriever,
        IRetryPolicyExceptionHandler retryPolicyExceptionHandler)
    {
        _retryRetriever = retryRetriever;
        _retryPolicyExceptionHandler = retryPolicyExceptionHandler;
    }

    /// <summary> Handle errors. </summary>
    public Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next) where TRequest : IRequest
    {
        return _retryRetriever.ExecuteAsync<Task>(c => next(request, c),
            _retryPolicyExceptionHandler.NeedToRetryTheException, cancellationToken);
    }

    /// <summary> Handle errors. </summary>
    public Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next) where TRequest : IRequest<TResponse>
    {
        return _retryRetriever.ExecuteAsync(c => next(request, c),
            _retryPolicyExceptionHandler.NeedToRetryTheException, cancellationToken);
    }

    /// <summary> Handle errors. </summary>
    public Task<ApplicationResult<TResponse>> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<ApplicationResult<TResponse>>> next)
        where TRequest : IRequest<ApplicationResult<TResponse>>
    {
        return _retryRetriever.ExecuteAsync(c => next(request, c),
            _retryPolicyExceptionHandler.NeedToRetryTheException, cancellationToken);
    }
}
