using Polly;
using SharedKernel.Application.Logging;
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
    public class PollyRetrieverWrap : IRetriever
    {
        private readonly ICustomLogger<PollyRetrieverWrap> _logger;
        private readonly RetrieverOptions _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public PollyRetrieverWrap(
            ICustomLogger<PollyRetrieverWrap> logger,
            IOptionsService<RetrieverOptions> options)
        {
            _logger = logger;
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
        public async Task ExecuteAsync<TResult>(Func<CancellationToken, Task> action,
            Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken)
        {
            var hasFallback = false;
            Exception ex = null;

            var fallbackPolicy = Policy.Handle(needToRetryTheException).FallbackAsync(
                _ => Task.CompletedTask, d =>
                {
                    _logger.Error(d, d.Message);
                    ex = d;

                    hasFallback = true;
                    return Task.FromResult(new { });

                });

            var retryPolicy = Policy
                .Handle(needToRetryTheException)
                .WaitAndRetryAsync(_options.RetryCount, _options.RetryAttempt());

            await fallbackPolicy.WrapAsync(retryPolicy).ExecuteAsync(action, cancellationToken);

            if (hasFallback && ex != null)
                throw new FallbackException(ex);
        }

        /// <summary>
        /// Retry retriever provides an ability to automatically re-invoke a failed operation
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="needToRetryTheException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action,
            Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken)
        {
            var hasFallback = false;
            Exception ex = null;

            var fallbackPolicy = Policy<TResult>.Handle<Exception>().FallbackAsync(
                _ => Task.FromResult<TResult>(default), d =>
                {
                    _logger.Error(d.Exception, d.Exception.Message);
                    ex = d.Exception;

                    hasFallback = true;
                    return Task.FromResult(new { });

                });

            var retryPolicy = Policy
                .Handle(needToRetryTheException)
                .WaitAndRetryAsync(_options.RetryCount, _options.RetryAttempt());

            var result = await fallbackPolicy
                .WrapAsync(retryPolicy)
                .ExecuteAsync(action, cancellationToken);

            if (hasFallback && ex != null)
                throw new FallbackException(ex);

            return result;
        }
    }
}