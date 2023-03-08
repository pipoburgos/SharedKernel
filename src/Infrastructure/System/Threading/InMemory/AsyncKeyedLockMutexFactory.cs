using AsyncKeyedLock;
using SharedKernel.Application.System.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.System.Threading.InMemory
{
    internal class AsyncKeyedLockMutexFactory : IMutexFactory
    {
        private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;

        /// <summary>
        /// Mutex manager constructor
        /// </summary>
        /// <param name="asyncKeyedLocker"></param>
        public AsyncKeyedLockMutexFactory(AsyncKeyedLocker<string> asyncKeyedLocker)
        {
            _asyncKeyedLocker = asyncKeyedLocker;
        }

        public IMutex Create(string key)
        {
            return new AsyncKeyedLockMutex(_asyncKeyedLocker.Lock(key));
        }

        public async Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken)
        {
            var disposable = await _asyncKeyedLocker.LockAsync(key, cancellationToken).ConfigureAwait(false);

            return new AsyncKeyedLockMutex(disposable);
        }
    }
}
