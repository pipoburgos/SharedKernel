using Medallion.Threading.Redis;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.Redis.System.Threading;

/// <summary> Redis mutex. </summary>
internal class RedisMutex : IMutex
{
    private readonly RedisDistributedLockHandle _redisDistributedLockHandle;

    /// <summary> Constructor. </summary>
    /// <param name="redisDistributedLockHandle"></param>
    public RedisMutex(RedisDistributedLockHandle redisDistributedLockHandle)
    {
        _redisDistributedLockHandle = redisDistributedLockHandle;
    }

    /// <summary> Release Sql Server mutex. </summary>
    public void Release()
    {
        _redisDistributedLockHandle.Dispose();
    }
}