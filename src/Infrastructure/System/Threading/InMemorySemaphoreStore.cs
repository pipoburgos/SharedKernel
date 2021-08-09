using System.Collections.Generic;
using System.Threading;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class InMemorySemaphoreStore : ISemaphoreStore
    {
        private readonly Dictionary<object, RefCounted<SemaphoreSlim>> _semaphoreSlims;

        /// <summary>
        /// 
        /// </summary>
        public InMemorySemaphoreStore()
        {
            _semaphoreSlims = new Dictionary<object, RefCounted<SemaphoreSlim>>();
        }

        private sealed class RefCounted<T>
        {
            internal RefCounted(T value)
            {
                RefCount = 1;
                Value = value;
            }

            public int RefCount { get; set; }
            public T Value { get; private set; }
        }

        /// <summary>
        /// Gets o create a SemaphoreSlim per key (Thread Safe)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SemaphoreSlim GetOrCreate(string key)
        {
            RefCounted<SemaphoreSlim> item;
            lock (_semaphoreSlims)
            {
                if (_semaphoreSlims.TryGetValue(key, out item))
                {
                    ++item.RefCount;
                }
                else
                {
                    item = new RefCounted<SemaphoreSlim>(new SemaphoreSlim(1, 1));
                    _semaphoreSlims[key] = item;
                }
            }
            return item.Value;
        }

        /// <summary>
        /// Remove a SemaphoreSlim per key (Thread Safe)
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            lock (_semaphoreSlims)
            {
                _semaphoreSlims.Remove(key);
            }
        }
    }
}
