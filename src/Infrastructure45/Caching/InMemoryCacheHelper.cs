using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Caching
{
    public class InMemoryCacheHelper : ICacheHelper
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICustomLogger<InMemoryCacheHelper> _customLogger;

        public InMemoryCacheHelper(IMemoryCache memoryCache, ICustomLogger<InMemoryCacheHelper> customLogger)
        {
            _memoryCache = memoryCache;
            _customLogger = customLogger;
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, int minutes = 600)
        {
            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                _customLogger.Verbose($"Ejecutanto de la cache {key}");
                entry.SlidingExpiration = TimeSpan.FromMinutes(minutes);
                return generator();
            });
        }
    }
}
