using SharedKernel.Application.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading;

public abstract class CommonMutexTests<TApp> : IClassFixture<TApp> where TApp : InfrastructureTestCase<FakeStartup>
{
    private readonly TApp _app1Mutex;
    private readonly TApp _app2Mutex;

    public CommonMutexTests(TApp app1Mutex, TApp app2Mutex)
    {
        _app1Mutex = app1Mutex;
        _app2Mutex = app2Mutex;
    }

    [Fact]
    public void MutexTest()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var firstTaskCompleted = false;
        Task.Run(() => mutexFactory1.RunOneAtATimeFromGivenKey("MutexKey", () =>
        {
            Thread.Sleep(250);
            firstTaskCompleted = true;
        }));

        Task.Run(() => mutexFactory1.RunOneAtATimeFromGivenKey("MutexKeyDistinct", () =>
        {
            firstTaskCompleted.Should().BeFalse();
        }));

        Task.Run(() => mutexFactory2.RunOneAtATimeFromGivenKey("MutexKey", () =>
        {
            firstTaskCompleted.Should().BeTrue();
        }));

    }

    [Fact]
    public async Task MutexTestAsync()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var tasks = new List<Task>();

        var firstTaskCompleted = false;
        tasks.Add(mutexFactory1.RunOneAtATimeFromGivenKeyAsync("MutexKey", async () =>
        {
            await Task.Delay(250);
            firstTaskCompleted = true;
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.Delay(10);

        tasks.Add(mutexFactory1.RunOneAtATimeFromGivenKeyAsync("MutexKeyDistinct", async () =>
        {
            await Task.Delay(0);
            firstTaskCompleted.Should().BeFalse();
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.Delay(10);

        tasks.Add(mutexFactory2.RunOneAtATimeFromGivenKeyAsync("MutexKey", async () =>
        {
            await Task.Delay(0);
            firstTaskCompleted.Should().BeTrue();
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.WhenAll(tasks);
    }
}