namespace SharedKernel.Domain.Entities;

/// <summary> . </summary>
public class AuditChange : AggregateRoot<Guid>
{
    /// <summary> . </summary>
    protected AuditChange() { }

    /// <summary> . </summary>
    protected AuditChange(Guid id, string registryId, string table, string property, string? originalValue,
        string? currentValue, DateTime date, State state)
    {
        Id = id;
        RegistryId = registryId;
        Table = table;
        Property = property;
        OriginalValue = originalValue;
        CurrentValue = currentValue;
        Date = date;
        State = state;
    }

    /// <summary> . </summary>
    public static AuditChange Create(Guid id, string registryId, string table, string property, string? originalValue,
        string? currentValue, DateTime date, State state)
    {
        return new AuditChange(id, registryId, table, property, originalValue, currentValue, date, state);
    }

    /// <summary> . </summary>
    public string RegistryId { get; private set; } = null!;

    /// <summary> . </summary>
    public string Table { get; private set; } = null!;

    /// <summary> . </summary>
    public string Property { get; private set; } = null!;

    /// <summary> . </summary>
    public string? OriginalValue { get; private set; }

    /// <summary> . </summary>
    public string? CurrentValue { get; private set; }

    /// <summary> . </summary>
    public DateTime Date { get; private set; }

    /// <summary> . </summary>
    public State State { get; private set; }
}
