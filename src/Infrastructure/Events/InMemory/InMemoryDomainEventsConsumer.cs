using SharedKernel.Application.Events;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.Events.InMemory;

/// <summary> </summary>
public class InMemoryDomainEventsConsumer : IInMemoryDomainEventsConsumer
{
    private readonly EventQueue _eventQueue;
    private readonly IRequestMediator _requestMediator;
    private readonly IRequestSerializer _requestSerializer;

    /// <summary>  </summary>
    public InMemoryDomainEventsConsumer(
        EventQueue eventQueue,
        IRequestMediator requestMediator,
        IRequestSerializer requestSerializer)
    {
        _eventQueue = eventQueue;
        _requestMediator = requestMediator;
        _requestSerializer = requestSerializer;
    }

    /// <summary>  </summary>
    /// <param name="domainEvent"></param>
    public void Add(DomainEvent domainEvent)
    {
        var eventSerialized = _requestSerializer.Serialize(domainEvent);
        _eventQueue.Enqueue(eventSerialized);
    }

    /// <summary>  </summary>
    /// <param name="domainEvents"></param>
    public void AddRange(IEnumerable<DomainEvent> domainEvents)
    {
        foreach (var @event in domainEvents)
        {
            var eventSerialized = _requestSerializer.Serialize(@event);
            _eventQueue.Enqueue(eventSerialized);
        }
    }

    /// <summary>  </summary>
    public async Task ExecuteAll(CancellationToken cancellationToken)
    {
        while (_eventQueue.TryDequeue(out var domainEvent))
        {
            await _requestMediator.Execute(domainEvent, typeof(IDomainEventSubscriber<>),
                nameof(IDomainEventSubscriber<DomainEvent>.On), cancellationToken);
        }

        await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
    }
}
