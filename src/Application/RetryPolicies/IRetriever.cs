using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.RetryPolicies
{
    /// <summary>
    /// Retry retriever provides an ability to automatically re-invoke a failed operation
    /// </summary>
    public interface IRetriever
    {
        /// <summary>
        /// Retry retriever provides an ability to automatically re-invoke a failed operation
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="needToRetryTheException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync<TResult>(Func<CancellationToken, Task> action,
            Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken);

        /// <summary>
        /// Retry retriever provides an ability to automatically re-invoke a failed operation
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="needToRetryTheException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action,
            Func<Exception, bool> needToRetryTheException, CancellationToken cancellationToken);
    }
}