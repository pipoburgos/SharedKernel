using FluentAssertions;
using SharedKernel.Application.System.Threading;
using SharedKernel.Testing.Infrastructure;
using System.Diagnostics;
using Xunit;

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
    public async Task MutexTestSync()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        const int time = 7_000;

        var timer = new Stopwatch();
        timer.Start();

        var tasks = new List<Task>
        {
            Task.Run(() => mutexFactory1.RunOneAtATimeFromGivenKey("MutexKey", () => Thread.Sleep(time))),
            Task.Run(() => mutexFactory2.RunOneAtATimeFromGivenKey("MutexKeyDistinct", () => Thread.Sleep(time))),
            Task.Run(() => mutexFactory2.RunOneAtATimeFromGivenKey("MutexKey", () => Thread.Sleep(time)))
        };

        await Task.WhenAll(tasks);

        timer.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(time * 2);
        timer.ElapsedMilliseconds.Should().BeLessOrEqualTo(time * 3);
    }

    [Fact]
    public async Task MutexTestAsync()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        const int time = 7_000;

        var tasks = new List<Task>();

        var timer = new Stopwatch();
        timer.Start();

        tasks.Add(mutexFactory1.RunOneAtATimeFromGivenKeyAsync("MutexKey", async () =>
        {
            await Task.Delay(time);
            return Task.CompletedTask;
        }, CancellationToken.None));

        tasks.Add(mutexFactory2.RunOneAtATimeFromGivenKeyAsync("MutexKeyDistinct", async () =>
        {
            await Task.Delay(time);
            return Task.CompletedTask;
        }, CancellationToken.None));

        tasks.Add(mutexFactory2.RunOneAtATimeFromGivenKeyAsync("MutexKey", async () =>
        {
            await Task.Delay(time);
            return Task.CompletedTask;
        }, CancellationToken.None));

        await Task.WhenAll(tasks);

        timer.Stop();
        timer.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(time * 2);
        timer.ElapsedMilliseconds.Should().BeLessOrEqualTo(time * 3);
    }
}