using Polly;
using Polly.Retry;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.RetryPolicies
{
    /// <summary>
    /// Retry retriever provides an ability to automatically re-invoke a failed operation
    /// </summary>
    public class PollyRetriever : IRetriever
    {
        private readonly RetrieverOptions _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public PollyRetriever(
            IOptionsService<RetrieverOptions> options)
        {
            _options = options.Value;
        }

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
                .WaitAndRetryAsync(_options.RetryCount, _options.RetryAttempt());
        }
    }
}