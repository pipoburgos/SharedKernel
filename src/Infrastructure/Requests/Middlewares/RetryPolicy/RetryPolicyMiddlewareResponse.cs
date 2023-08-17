using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Requests;
using SharedKernel.Application.RetryPolicies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares.RetryPolicy;

/// <summary>
/// Retry retriever provides an ability to automatically re-invoke a failed operation
/// </summary>
public class RetryPolicyMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
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
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next)
    {
        return _retryRetriever.ExecuteAsync(c => next(request, c),
            _retryPolicyExceptionHandler.NeedToRetryTheException, cancellationToken);
    }
}