using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using SharedKernel.Application.RetryPolicies;

namespace SharedKernel.Infrastructure.RetryPolicies
{
    /// <summary>
    /// Retry retriever provides an ability to automatically re-invoke a failed operation
    /// </summary>
    public class PollyRetriever : IRetriever
    {
        /// <summary>
        /// Retry retriever provides an ability to automatically re-invoke a failed operation
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="needToRetryTheException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ExecuteAsync<TResult>(Func<CancellationToken, Task> action,
            Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken)
        {
            return Common(needToRetryTheException).ExecuteAsync(action, cancellationToken);
        }

        /// <summary>
        /// Retry retriever provides an ability to automatically re-invoke a failed operation
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="needToRetryTheException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action,
            Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken)

        {
            return Common(needToRetryTheException).ExecuteAsync(action, cancellationToken);
        }

        private AsyncRetryPolicy Common(Func<Exception, bool> needToRetryTheException)
        {
            return Policy
                .Handle(needToRetryTheException)
                .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100));
        }
    }
}