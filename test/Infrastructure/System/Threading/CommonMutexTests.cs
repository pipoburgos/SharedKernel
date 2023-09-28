using SharedKernel.Application.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading;

public abstract class CommonMutexTests<TApp> : IClassFixture<TApp> where TApp : InfrastructureTestCase<FakeStartup>
{
    private readonly TApp _app1Mutex;
    private readonly TApp _app2Mutex;
    private const int Delay = 50;

    public CommonMutexTests(TApp app1Mutex, TApp app2Mutex)
    {
        _app1Mutex = app1Mutex;
        _app2Mutex = app2Mutex;
    }

    [Fact]
    public async Task MutexTest()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var tasks = new List<Task>();

        var firstTaskCompleted = false;

        tasks.Add(Task.Run(() => mutexFactory1.RunOneAtATimeFromGivenKey("MutexKey", () =>
        {
            Thread.Sleep(5_000);
            firstTaskCompleted = true;
        })));

        await Task.Delay(Delay);

        tasks.Add(Task.Run(() =>
            mutexFactory1.RunOneAtATimeFromGivenKey("MutexKeyDistinct", () => firstTaskCompleted.Should().BeFalse())));

        await Task.Delay(Delay);

        tasks.Add(Task.Run(() =>
            mutexFactory2.RunOneAtATimeFromGivenKey("MutexKey", () => firstTaskCompleted.Should().BeTrue())));

        await Task.WhenAll(tasks);

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
            await Task.Delay(5_000);
            firstTaskCompleted = true;
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.Delay(Delay);

        tasks.Add(mutexFactory1.RunOneAtATimeFromGivenKeyAsync("MutexKeyDistinct", async () =>
        {
            await Task.Delay(0);
            firstTaskCompleted.Should().BeFalse();
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.Delay(Delay);

        tasks.Add(mutexFactory2.RunOneAtATimeFromGivenKeyAsync("MutexKey", async () =>
        {
            await Task.Delay(0);
            firstTaskCompleted.Should().BeTrue();
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.WhenAll(tasks);
    }
}