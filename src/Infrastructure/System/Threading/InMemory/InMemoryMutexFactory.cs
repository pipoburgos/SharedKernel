using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading.InMemory
{
    internal class InMemoryMutexFactory : IMutexFactory
    {
        private readonly Dictionary<object, RefCounted<SemaphoreSlim>> _semaphoreSlims;

        public sealed class RefCounted<T>
        {
            public RefCounted(T value)
            {
                RefCount = 1;
                Value = value;
            }

            public int RefCount { get; set; }
            public T Value { get; private set; }
        }

        public InMemoryMutexFactory()
        {
            _semaphoreSlims = new Dictionary<object, RefCounted<SemaphoreSlim>>();
        }

        public IMutex Create(string key)
        {
            var item = CreateThreadSafe(key);

            item.Value.Wait();

            return new InMemoryMutex(item.Value);
        }

        public async Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken)
        {
            var item = CreateThreadSafe(key);

            await item.Value.WaitAsync(cancellationToken);

            return new InMemoryMutex(item.Value);
        }

        private RefCounted<SemaphoreSlim> CreateThreadSafe(string key)
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

            return item;
        }
    }
}
