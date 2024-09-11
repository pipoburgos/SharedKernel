using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.Caching;

/// <summary> . </summary>
public class DistributedCacheHelper : ICacheHelper
{
    private readonly IDistributedCache _distributedCache;
    private readonly IMutexManager _mutexManager;
    private readonly IBinarySerializer _binarySerializer;

    /// <summary> . </summary>
    public DistributedCacheHelper(
        IDistributedCache distributedCache,
        IMutexManager mutexManager,
        IBinarySerializer binarySerializer)
    {
        _distributedCache = distributedCache;
        _mutexManager = mutexManager;
        _binarySerializer = binarySerializer;
    }

    /// <summary> . </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : notnull
    {
        var value = await _distributedCache.GetAsync(key, cancellationToken);

        return value == default || value.Length == 0 ? default : _binarySerializer.Deserialize<T>(value);
    }

    /// <summary> . </summary>
    public Task SetAsync<T>(string key, T value, TimeSpan? timeSpan = default,
        CancellationToken cancellationToken = default) where T : notnull
    {
        return _mutexManager.RunOneAtATimeFromGivenKey(key,
            () => _distributedCache.SetAsync(key, _binarySerializer.Serialize(value),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeSpan }, cancellationToken));
    }

    /// <summary> . </summary>
    public Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        return _mutexManager.RunOneAtATimeFromGivenKeyAsync(key, async () =>
        {
            var value = await _distributedCache.GetAsync(key, cancellationToken);

            if (value != default && value.Length != 0)
                return _binarySerializer.Deserialize<T>(value);


            var valueToCache = await generator();
            await _distributedCache.SetAsync(key, _binarySerializer.Serialize(valueToCache),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeSpan }, cancellationToken);

            return valueToCache;
        }, cancellationToken);
    }

    /// <summary> . </summary>
    public void Remove(string key)
    {
        _distributedCache.Remove(key);
    }
}
