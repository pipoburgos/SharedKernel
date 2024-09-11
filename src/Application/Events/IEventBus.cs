namespace SharedKernel.Application.Events;

/// <summary> . </summary>
public interface IEventBus
{
    /// <summary> Publish an event to event bus. </summary>
    Task Publish(DomainEvent @event, CancellationToken cancellationToken);

    /// <summary> Publish a list of events to event bus. </summary>
    Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken);
}
