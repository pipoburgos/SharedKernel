using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>
    /// Retry retriever provides an ability to automatically re-invoke a failed operation
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class RetryPolicyMiddleware<TRequest> : IMiddleware<TRequest> where TRequest : IRequest
    {
        private readonly IRetriever _retryRetriever;
        private readonly IRetryPolicyExceptionHandler _retryPolicyExceptionHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="retryRetriever"></param>
        /// <param name="retryPolicyExceptionHandler"></param>
        public RetryPolicyMiddleware(
            IRetriever retryRetriever,
            IRetryPolicyExceptionHandler retryPolicyExceptionHandler)
        {
            _retryRetriever = retryRetriever;
            _retryPolicyExceptionHandler = retryPolicyExceptionHandler;
        }

        /// <summary>
        /// Handle errors
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task Handle(TRequest request, CancellationToken cancellationToken,
            Func<TRequest, CancellationToken, Task> next)
        {
            return _retryRetriever.ExecuteAsync<Task>(c => next(request, c),
                _retryPolicyExceptionHandler.NeedToRetryTheException, cancellationToken);
        }
    }
}
