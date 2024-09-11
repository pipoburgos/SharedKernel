namespace SharedKernel.Domain.Aggregates;

/// <summary> . </summary>
public abstract class AggregateRootAuditableLogicalRemove<TId> : AggregateRootAuditable<TId>,
    IEntityAuditableLogicalRemove where TId : notnull
{
    /// <summary> . </summary>
    protected AggregateRootAuditableLogicalRemove() { }

    /// <summary> . </summary>
    protected AggregateRootAuditableLogicalRemove(TId id) : base(id) { }

    /// <summary> . </summary>
    public Guid? DeletedBy { get; private set; }

    /// <summary> . </summary>
    public DateTime? DeletedAt { get; private set; }


    /// <summary> . </summary>
    public virtual void Delete(DateTime deletedAt, Guid userId)
    {
        DeletedAt = deletedAt;
        DeletedBy = userId;
    }

    /// <summary> . </summary>
    public virtual void Restore()
    {
        DeletedBy = null;
        DeletedAt = null;
    }
}
