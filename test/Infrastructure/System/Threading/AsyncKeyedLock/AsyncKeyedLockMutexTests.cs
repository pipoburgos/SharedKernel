namespace SharedKernel.Integration.Tests.System.Threading.AsyncKeyedLock;

public class AsyncKeyedLockMutexTests : CommonMutexTests<AsyncKeyedLockApp>
{
    public AsyncKeyedLockMutexTests(AsyncKeyedLockApp app1Mutex) : base(app1Mutex, app1Mutex)
    {
    }
}
