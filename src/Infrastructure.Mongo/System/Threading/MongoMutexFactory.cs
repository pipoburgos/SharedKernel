using DistributedLock.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.Application.System.Threading;
using SharedKernel.Infrastructure.Mongo.Data;

namespace SharedKernel.Infrastructure.Mongo.System.Threading;

internal class MongoMutexFactory : IMutexFactory
{
    private readonly IOptions<MongoSettings> _options;

    public MongoMutexFactory(IOptions<MongoSettings> options)
    {
        _options = options;
    }

    public IDisposable Create(string key)
    {
        return CreateAsync(key, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var @lock = GetMongoLock(key);
        var acquire = await @lock.AcquireAsync(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(10));
        try
        {
            return acquire?.Acquired == true ? new MongoMutex(@lock, acquire) : new MongoMutex();
        }
        finally
        {
            await @lock.ReleaseAsync(acquire);
        }
    }

    /// <summary> https://www.nuget.org/packages/DistributedLock.Mongo </summary>
    private MongoLock<string> GetMongoLock(string key)
    {
        var client = new MongoClient(_options.Value.ConnectionString);
        var database = client.GetDatabase(_options.Value.Database);

        var locks = database.GetCollection<LockAcquire<string>>("locks");

        // This collection should be a capped! https://docs.mongodb.com/manual/core/capped-collections/
        // The size of the capped collection should be enough to put all active locks.
        // One ReleaseSignal is about 32 bytes, so for 100,000 simultaneously locks,
        // you need a capped collection size ~3 megabytes
        var signals = database.GetCollection<ReleaseSignal>("signals");

        var @lock = new MongoLock<string>(locks, signals, key);
        return @lock;
    }
}
