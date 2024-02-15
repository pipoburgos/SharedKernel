namespace SharedKernel.Domain.Aggregates;

/// <summary> This root aggregate contains your domain identifier and events. </summary>
/// <typeparam name="TId"> The data type of the identifier. </typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents;

    /// <summary> Aggregate Root constructor for ORMs. </summary>
    protected AggregateRoot()
    {
        _domainEvents = new List<DomainEvent>();
    }

    /// <summary> Aggregate Root constructor. </summary>
    /// <param name="id">The identifier</param>
    protected AggregateRoot(TId id)
    {
        Id = id;
        _domainEvents = new List<DomainEvent>();
    }

    /// <summary> Extracts the domain events that have the root aggregate. </summary>
    /// <returns>All domain events recordered</returns>
    public List<DomainEvent> PullDomainEvents()
    {
        if (Id is int)
            _domainEvents.ForEach(e => e.SetAggregateId(Id.ToString()!));

        var events = _domainEvents.ToList();
        _domainEvents.Clear();
        return events;
    }

    /// <summary> Stores a domain event for later extraction. </summary>
    /// <param name="domainEvent"></param>
    public void Record(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
