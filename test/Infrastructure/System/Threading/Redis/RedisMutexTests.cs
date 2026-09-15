namespace SharedKernel.Integration.Tests.System.Threading.Redis;


public class RedisMutexTests : CommonMutexTests<RedisApp>
{
    public RedisMutexTests(TwoAppFixture<RedisApp> fixture) : base(fixture)
    {
    }
}
