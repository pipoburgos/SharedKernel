﻿namespace SharedKernel.Integration.Tests.System.Threading.Redis;


public class RedisMutexTests : CommonMutexTests<RedisApp>
{
    public RedisMutexTests(RedisApp app1Mutex, RedisApp app2Mutex) : base(app1Mutex, app2Mutex)
    {
    }
}
