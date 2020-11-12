using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Caching
{
    internal class InMemoryCacheHelper : ICacheHelper
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICustomLogger<InMemoryCacheHelper> _customLogger;

        public InMemoryCacheHelper(IMemoryCache memoryCache, ICustomLogger<InMemoryCacheHelper> customLogger)
        {
            _memoryCache = memoryCache;
            _customLogger = customLogger;
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null)
        {
            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                _customLogger.Verbose($"Ejecutanto de la cache {key}");
                entry.SlidingExpiration = timeSpan;
                return generator();
            });
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
