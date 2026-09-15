using SharedKernel.Application.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading;

public abstract class CommonMutexTests<TApp> : IClassFixture<TwoAppFixture<TApp>>
    where TApp : InfrastructureTestCase<FakeStartup>, new()
{
    private readonly TApp _app1Mutex;
    private readonly TApp _app2Mutex;

    protected CommonMutexTests(TwoAppFixture<TApp> apps)
    {
        _app1Mutex = apps.App1;
        _app2Mutex = apps.App2;
    }

    [Fact]
    public async Task MutexTest()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();

        var mutexManager1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexManager2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var firstEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var releaseFirst = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var secondEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var firstCompleted = false;

        var firstTask = Task.Run(() =>
        {
            mutexManager1.RunOneAtATimeFromGivenKey(
                "MutexKey",
                () =>
                {
                    firstEntered.SetResult();

                    releaseFirst.Task
                        .GetAwaiter()
                        .GetResult();

                    Volatile.Write(ref firstCompleted, true);
                });
        });

        // Garantizamos que el primer callback está dentro del mutex.
        await firstEntered.Task;

        var secondTask = Task.Run(() =>
        {
            mutexManager2.RunOneAtATimeFromGivenKey(
                "MutexKey",
                () =>
                {
                    // El segundo callback solamente puede entrar
                    // después de que haya terminado el primero.
                    Volatile.Read(ref firstCompleted)
                        .Should()
                        .BeTrue();

                    secondEntered.SetResult();
                });
        });

        /*
         * IMPORTANTE:
         *
         * No esperamos aquí al segundo.
         *
         * El primero sigue reteniendo el mutex, por lo que el segundo
         * debe estar esperando.
         *
         * Lo liberamos explícitamente.
         */
        releaseFirst.SetResult();

        await Task.WhenAll(firstTask, secondTask);

        firstCompleted.Should().BeTrue();
        secondEntered.Task.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task MutexTestAsync()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();

        var mutexManager1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexManager2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var firstEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var releaseFirst = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var secondEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var firstCompleted = false;

        var firstTask =
            mutexManager1.RunOneAtATimeFromGivenKeyAsync(
                "MutexKey",
                async () =>
                {
                    firstEntered.SetResult();

                    await releaseFirst.Task;

                    Volatile.Write(ref firstCompleted, true);

                    return true;
                },
                CancellationToken.None);

        // Garantizamos que el primer callback está dentro del mutex.
        await firstEntered.Task;

        var secondTask =
            mutexManager2.RunOneAtATimeFromGivenKeyAsync(
                "MutexKey",
                async () =>
                {
                    // El segundo solamente puede entrar después
                    // de que haya terminado el primero.
                    Volatile.Read(ref firstCompleted)
                        .Should()
                        .BeTrue();

                    secondEntered.SetResult();

                    return true;
                },
                CancellationToken.None);

        /*
         * Liberamos el primer mutex.
         *
         * Si la implementación no bloquea correctamente por clave,
         * el callback de secondTask llegará aquí antes de que
         * firstCompleted sea true y la aserción fallará.
         */
        releaseFirst.SetResult();

        await Task.WhenAll(firstTask, secondTask);

        firstCompleted.Should().BeTrue();
        secondEntered.Task.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task MutexDifferentKeysCanRunAtTheSameTime()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();

        var mutexManager1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexManager2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var firstEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var releaseFirst = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var secondEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var firstCompleted = false;

        var firstTask = Task.Run(() =>
        {
            mutexManager1.RunOneAtATimeFromGivenKey(
                "MutexKey",
                () =>
                {
                    firstEntered.SetResult();

                    releaseFirst.Task
                        .GetAwaiter()
                        .GetResult();

                    Volatile.Write(ref firstCompleted, true);
                });
        });

        // Primer mutex adquirido.
        await firstEntered.Task;

        var secondTask = Task.Run(() =>
        {
            mutexManager2.RunOneAtATimeFromGivenKey(
                "MutexKeyDistinct",
                () =>
                {
                    /*
                     * La primera operación sigue dentro del mutex.
                     *
                     * Si las claves son independientes, este callback
                     * puede ejecutarse ahora.
                     */
                    Volatile.Read(ref firstCompleted)
                        .Should()
                        .BeFalse();

                    secondEntered.SetResult();
                });
        });

        /*
         * ESPERAMOS AL CALLBACK, no a que termine secondTask.
         *
         * Esto demuestra que la segunda clave ha podido entrar
         * mientras la primera sigue bloqueada.
         */
        await secondEntered.Task;

        firstCompleted.Should().BeFalse();

        // Ahora liberamos la primera clave.
        releaseFirst.SetResult();

        await Task.WhenAll(firstTask, secondTask);

        firstCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task MutexDifferentKeysCanRunAtTheSameTimeAsync()
    {
        _app1Mutex.BeforeStart();
        _app2Mutex.BeforeStart();

        var mutexManager1 = _app1Mutex.GetRequiredService<IMutexManager>();
        var mutexManager2 = _app2Mutex.GetRequiredService<IMutexManager>();

        var firstEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var releaseFirst = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var secondEntered = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var firstCompleted = false;

        var firstTask =
            mutexManager1.RunOneAtATimeFromGivenKeyAsync(
                "MutexKey",
                async () =>
                {
                    firstEntered.SetResult();

                    await releaseFirst.Task;

                    Volatile.Write(ref firstCompleted, true);

                    return true;
                },
                CancellationToken.None);

        // Primer mutex adquirido.
        await firstEntered.Task;

        var secondTask =
            mutexManager2.RunOneAtATimeFromGivenKeyAsync(
                "MutexKeyDistinct",
                async () =>
                {
                    /*
                     * MutexKey sigue ocupado, pero esta clave es distinta,
                     * por lo que debemos poder entrar.
                     */
                    Volatile.Read(ref firstCompleted)
                        .Should()
                        .BeFalse();

                    secondEntered.SetResult();

                    return true;
                },
                CancellationToken.None);

        /*
         * Si las claves son independientes, esto termina mientras
         * el primer callback sigue esperando releaseFirst.
         */
        await secondEntered.Task;

        firstCompleted.Should().BeFalse();

        // Liberamos el primer mutex.
        releaseFirst.SetResult();

        await Task.WhenAll(firstTask, secondTask);

        firstCompleted.Should().BeTrue();
    }
}