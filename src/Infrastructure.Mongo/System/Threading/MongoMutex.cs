using DistributedLock.Mongo;

namespace SharedKernel.Infrastructure.Mongo.System.Threading;
internal class MongoMutex : IDisposable
{
    private readonly MongoLock<string>? _mongoLock;
    private readonly IAcquire? _acquire;

    public MongoMutex()
    {
    }

    public MongoMutex(MongoLock<string> mongoLock, IAcquire acquire)
    {
        _mongoLock = mongoLock;
        _acquire = acquire;
    }

    public void Dispose()
    {
        _mongoLock?.ReleaseAsync(_acquire).GetAwaiter().GetResult();
    }
}
