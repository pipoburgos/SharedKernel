using SharedKernel.Application.System.Threading;
using System;

namespace SharedKernel.Infrastructure.System.Threading.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncKeyedLockMutex : IMutex
    {
        private readonly IDisposable _disposable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposable"></param>
        public AsyncKeyedLockMutex(IDisposable disposable)
        {
            _disposable = disposable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
            _disposable.Dispose();
        }
    }
}
