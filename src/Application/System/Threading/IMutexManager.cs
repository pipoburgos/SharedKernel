using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// Mutex manager
    /// </summary>
    public interface IMutexManager
    {
        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        void RunOneAtATimeFromGivenKey(string key, Action action);

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        T RunOneAtATimeFromGivenKey<T>(string key, Func<T> function);

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="function"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> RunOneAtATimeFromGivenKeyAsync<T>(string key, Func<Task<T>> function, CancellationToken cancellationToken);
    }
}