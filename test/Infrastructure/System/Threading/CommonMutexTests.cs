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
    public void MutexTest()
    {
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        const int total = 10;
        const int time = 250;

        var timer = new Stopwatch();

        timer.Start();

        for (var i = 0; i < total; i++)
        {
            mutexFactory1.RunOneAtATimeFromGivenKey("MutexText", () => Thread.Sleep(time));

            mutexFactory1.RunOneAtATimeFromGivenKey("MutexTextDistinct", () => Thread.Sleep(time));

            mutexFactory2.RunOneAtATimeFromGivenKey("MutexText", () => Thread.Sleep(time));
        }

        timer.Stop();
        timer.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(time * total);
    }

    [Fact]
    public async Task MutexTestAsync()
    {
        var mutexFactory1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexFactory2 = _app2Mutex.GetRequiredService<IMutexManager>();

        const int total = 10;
        const int time = 250;

        var tasks = new List<Task>();

        var timer = new Stopwatch();

        timer.Start();

        for (var i = 0; i < total; i++)
        {
            tasks.Add(mutexFactory1.RunOneAtATimeFromGivenKeyAsync("MutexText", async () =>
                {
                    await Task.Delay(time);
                    return Task.CompletedTask;
                },
                CancellationToken.None));

            tasks.Add(mutexFactory1.RunOneAtATimeFromGivenKeyAsync("MutexTextDistinct", async () =>
                {
                    await Task.Delay(time);
                    return Task.CompletedTask;
                },
                CancellationToken.None));

            tasks.Add(mutexFactory2.RunOneAtATimeFromGivenKeyAsync("MutexText", async () =>
                {
                    await Task.Delay(time);
                    return Task.CompletedTask;
                },
                CancellationToken.None));
        }

        await Task.WhenAll(tasks);

        timer.Stop();
        timer.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(time * total);
    }
}