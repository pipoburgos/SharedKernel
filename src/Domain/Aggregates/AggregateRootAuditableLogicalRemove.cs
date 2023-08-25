namespace SharedKernel.Domain.Aggregates;

/// <summary>  </summary>
public abstract class AggregateRootAuditableLogicalRemove<TKey> : AggregateRootAuditable<TKey>,
    IEntityAuditableLogicalRemove where TKey : notnull
{
    /// <summary>  </summary>
    protected AggregateRootAuditableLogicalRemove() { }

    /// <summary>  </summary>
    protected AggregateRootAuditableLogicalRemove(TKey id) : base(id) { }

    /// <summary>  </summary>
    public Guid? DeletedBy { get; private set; }

    /// <summary>  </summary>
    public DateTime? DeletedAt { get; private set; }


    /// <summary>  </summary>
    public virtual void Delete(DateTime deletedAt, Guid deletedBy)
    {
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
    }

    /// <summary>  </summary>
    public virtual void Restore()
    {
        DeletedBy = null;
        DeletedAt = null;
    }
}
