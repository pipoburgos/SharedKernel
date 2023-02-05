using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AsyncKeyedLock;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading
{
    /// <summary>
    /// Mutex manager
    /// </summary>
    public class MutexManager : IMutexManager
    {
        private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;

        /// <summary>
        /// Mutex manager constructor
        /// </summary>
        /// <param name="asyncKeyedLocker"></param>
        public MutexManager(AsyncKeyedLocker<string> asyncKeyedLocker)
        {
            _asyncKeyedLocker = asyncKeyedLocker;
        }

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RunOneAtATimeFromGivenKey(string key, Action action)
        {
            using (_asyncKeyedLocker.Lock(key))
            {
                action();
            }
        }

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T RunOneAtATimeFromGivenKey<T>(string key, Func<T> function)
        {
            using (_asyncKeyedLocker.Lock(key))
            {
                return function();
            }
        }

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="function"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<T> RunOneAtATimeFromGivenKeyAsync<T>(string key, Func<Task<T>> function, CancellationToken cancellationToken)
        {
            using (await _asyncKeyedLocker.LockAsync(key, cancellationToken).ConfigureAwait(false))
            {
                return await function();
            }
        }
    }
}