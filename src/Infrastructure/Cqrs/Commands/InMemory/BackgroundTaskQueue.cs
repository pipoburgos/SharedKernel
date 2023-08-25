using System.Collections.Concurrent;

namespace SharedKernel.Infrastructure.Cqrs.Commands.InMemory;

/// <summary>  </summary>
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly SemaphoreSlim _signal = new(0);

    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();

    /// <summary>  </summary>
    public bool Any()
    {
        return _workItems.Any();
    }

    /// <summary>  </summary>
    public void QueueBackground(Func<CancellationToken, Task> workItem)
    {
        if (workItem == null)
            throw new ArgumentNullException(nameof(workItem));

        _workItems.Enqueue(workItem);
        _signal.Release();
    }

    /// <summary>  </summary>
    public async Task<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _workItems.TryDequeue(out var workItem);

        return workItem ?? (_ => Task.CompletedTask);
    }
}
