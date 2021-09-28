using System.Threading;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading.InMemory
{
    /// <summary>
    /// In memory mutex
    /// </summary>
    public class InMemoryMutex : IMutex
    {
        private readonly SemaphoreSlim _semaphoreSlim;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        public InMemoryMutex(SemaphoreSlim semaphoreSlim)
        {
            _semaphoreSlim = semaphoreSlim;
        }

        /// <summary>
        /// Release in memory mutex
        /// </summary>
        public void Release()
        {
            _semaphoreSlim.Release();
        }
    }
}