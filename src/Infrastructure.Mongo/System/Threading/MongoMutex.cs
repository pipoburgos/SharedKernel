using DistributedLock.Mongo;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.Mongo.System.Threading;

/// <summary> Redis mutex. </summary>
internal class MongoMutex : IMutex
{
    private readonly MongoLock<Guid> _mongoLock;

    /// <summary> Constructor. </summary>
    public MongoMutex(MongoLock<Guid> mongoLock)
    {
        _mongoLock = mongoLock;
    }

    /// <summary> Release Sql Server mutex. </summary>
    public void Release()
    {
    }
}
    