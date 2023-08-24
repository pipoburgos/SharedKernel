namespace SharedKernel.Application.Events;

/// <summary>  </summary>
public interface IDomainEventSubscriber<in T> where T : DomainEvent
{
    /// <summary>  </summary>
    Task On(T @event, CancellationToken cancellationToken);
}
