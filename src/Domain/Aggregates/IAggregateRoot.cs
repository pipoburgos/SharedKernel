namespace SharedKernel.Domain.Aggregates;

/// <summary> This root aggregate contains your domain identifier and events. </summary>
public interface IAggregateRoot
{
    /// <summary> Extracts the domain events that have the root aggregate. </summary>
    /// <returns> All domain events recordered. </returns>
    List<DomainEvent> PullDomainEvents();
}
