using SharedKernel.Application.Events;

namespace SharedKernel.Infrastructure.Events.InMemory;

/// <summary> In memory event bus. </summary>
public class InMemoryEventBus : IEventBus
{
    private readonly IInMemoryDomainEventsConsumer _domainEventsToExecute;

    /// <summary> Constructor. </summary>
    public InMemoryEventBus(
        IInMemoryDomainEventsConsumer domainEventsToExecute)
    {
        _domainEventsToExecute = domainEventsToExecute;
    }

    /// <summary> Publish an event to event bus. </summary>
    public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
    {
        return Publish(new List<DomainEvent> { @event }, cancellationToken);
    }

    /// <summary> Publish an event to event bus. </summary>
    public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        if (events == default)
            return Task.CompletedTask;

        _domainEventsToExecute.AddRange(events);

        return Task.CompletedTask;
    }
}
