using Microsoft.Extensions.Caching.Memory;
using SharedKernel.Application.Caching;

namespace SharedKernel.Infrastructure.Caching;

internal class InMemoryCacheHelper : ICacheHelper
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<InMemoryCacheHelper> _logger;

    public InMemoryCacheHelper(IMemoryCache memoryCache, ILogger<InMemoryCacheHelper> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : notnull
    {
        return Task.FromResult(_memoryCache.Get<T>(key));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? timeSpan = default,
        CancellationToken cancellationToken = default) where T : notnull
    {
        _memoryCache.Set(key, value, timeSpan ?? TimeSpan.MaxValue);
        return Task.CompletedTask;
    }

    public Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = default,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        return _memoryCache.GetOrCreateAsync(key, entry =>
        {
            _logger.LogTrace($"Retrieving from cache {key}");
            entry.SlidingExpiration = timeSpan;
            return generator();
        });
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}
