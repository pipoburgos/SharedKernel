using System.Collections.Concurrent;

namespace SharedKernel.Infrastructure.Events.InMemory;

/// <summary> . </summary>
public class EventQueue
{
    private readonly ConcurrentQueue<string> _events;

    /// <summary> . </summary>
    public EventQueue()
    {
        _events = new ConcurrentQueue<string>();
    }

    /// <summary> . </summary>
    /// <param name="event"></param>
    public void Enqueue(string @event)
    {
        _events.Enqueue(@event);
    }

    /// <summary> . </summary>
    public bool TryDequeue(out string @event)
    {
        return _events.TryDequeue(out @event!);
    }
}