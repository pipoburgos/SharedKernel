using Medallion.Threading.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using SharedKernel.Application.System.Threading;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.System.Threading;

internal class RedisMutexFactory : IMutexFactory
{
    private readonly IOptions<RedisCacheOptions> _options;

    public RedisMutexFactory(IOptions<RedisCacheOptions> options)
    {
        _options = options;
    }

    public IDisposable Create(string key)
    {
        var redis = ConnectionMultiplexer.Connect(_options.Value.Configuration!);
        var @lock = new RedisDistributedLock(key, redis.GetDatabase());
        var sqlDistributedLockHandle = @lock.Acquire();
        return sqlDistributedLockHandle;
    }

    public async Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var redis = await ConnectionMultiplexer.ConnectAsync(_options.Value.Configuration!);
        var @lock = new RedisDistributedLock(key, redis.GetDatabase());
        var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
        return sqlDistributedLockHandle;
    }
}
