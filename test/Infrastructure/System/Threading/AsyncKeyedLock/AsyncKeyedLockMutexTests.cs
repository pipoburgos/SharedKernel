namespace SharedKernel.Integration.Tests.System.Threading.AsyncKeyedLock;

public class AsyncKeyedLockMutexTests : CommonMutexTests<AsyncKeyedLockApp>
{
    public AsyncKeyedLockMutexTests(TwoAppFixture<AsyncKeyedLockApp> fixture) : base(fixture)
    {
    }
}
