using DistributedLock.Mongo;
using Microsoft.Extensions.Options;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.Mongo.System.Threading;

internal class MongoMutexFactory : IMutexFactory
{
    private readonly IOptions<RedisCacheOptions> _options;

    public MongoMutexFactory(IOptions<RedisCacheOptions> options)
    {
        _options = options;
    }

    public IMutex Create(string key)
    {
        var @lock = new MongoLock<Guid>(key, GetDatabase());
        var sqlDistributedLockHandle = @lock.Acquire();
        return new MongoMutex(sqlDistributedLockHandle);
    }

    public async Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var @lock = new MongoLock(key, GetDatabase());
        var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
        return new MongoMutex(sqlDistributedLockHandle);
    }
}
