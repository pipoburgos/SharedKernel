using System;
#if !NET40
using System.Threading;
using System.Threading.Tasks;
#endif

namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// Mutex manager
    /// </summary>
    public class MutexManager : IMutexManager
    {
        private readonly IMutexFactory _lockerFactory;

        /// <summary>
        /// Mutex manager constructor
        /// </summary>
        /// <param name="lockerFactory"></param>
        public MutexManager(IMutexFactory lockerFactory)
        {
            _lockerFactory = lockerFactory;
        }

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void RunOneAtATimeFromGivenKey(string key, Action action)
        {
            var locker = _lockerFactory.Create(key);
            try
            {
                action();
            }
            finally
            {
                locker.Release();
            }
        }

        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public T RunOneAtATimeFromGivenKey<T>(string key, Func<T> function)
        {
            var locker = _lockerFactory.Create(key);
            try
            {
                return function();
            }
            finally
            {
                locker.Release();
            }
        }

#if !NET40
        /// <summary>
        /// Run one at time from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="function"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> RunOneAtATimeFromGivenKeyAsync<T>(string key, Func<Task<T>> function, CancellationToken cancellationToken)
        {
            var locker = await _lockerFactory.CreateAsync(key, cancellationToken);
            try
            {
                return await function();
            }
            finally
            {
                locker.Release();
            }
        }
#endif

    }
}