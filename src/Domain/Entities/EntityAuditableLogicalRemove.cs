namespace SharedKernel.Domain.Entities;

/// <summary>  </summary>
public abstract class EntityAuditableLogicalRemove<TKey> : EntityAuditable<TKey>, IEntityAuditableLogicalRemove where TKey : notnull
{
    /// <summary> Entity constructor for ORMs. </summary>
    protected EntityAuditableLogicalRemove() { }

    /// <summary> Constructor. </summary>
    protected EntityAuditableLogicalRemove(TKey id) : base(id) { }

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
