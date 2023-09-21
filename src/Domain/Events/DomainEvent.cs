namespace SharedKernel.Domain.Events;

/// <summary> Generic domain event. </summary>
public abstract class DomainEvent : Request
{
    #region Constructors

    /// <summary> Domain event serializable constructor. </summary>
    protected DomainEvent() { }

    /// <summary> Domain event constructor. </summary>
    protected DomainEvent(string? eventId = default, string? occurredOn = default) : base(eventId, occurredOn)
    {
    }

    /// <summary> Domain event constructor. </summary>
    protected DomainEvent(string aggregateId, string? eventId = default, string? occurredOn = default) : base(eventId, occurredOn)
    {
        AggregateId = aggregateId;
    }

    #endregion

    #region Properties

    /// <summary> The aggregate root identifier. </summary>
    public string AggregateId { get; private set; } = null!;

    #endregion

    #region Methods

    /// <summary> Sets aggregate id. </summary>
    public void SetAggregateId(string id)
    {
        if (string.IsNullOrWhiteSpace(AggregateId))
            AggregateId = id;
    }

    /// <summary> The event identifier for message queues. </summary>
    public override string GetUniqueName()
    {
        return GetEventName();
    }

    /// <summary> The event identifier for message queues. </summary>
    public abstract string GetEventName();

    /// <summary> Create a new Domain event with default values. </summary>
    public override Request FromPrimitives(Dictionary<string, string> body, string eventId, string occurredOn)
    {
        return FromPrimitives(body[nameof(AggregateId)], body, eventId, occurredOn);
    }

    /// <summary> Create a new Domain event with default values. </summary>
    public abstract DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
        string occurredOn);

    #endregion
}
