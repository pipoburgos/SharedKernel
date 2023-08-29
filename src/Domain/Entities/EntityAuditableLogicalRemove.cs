namespace SharedKernel.Domain.Entities;

/// <summary>  </summary>
public abstract class EntityAuditableLogicalRemove<TId> : EntityAuditable<TId>, IEntityAuditableLogicalRemove where TId : notnull
{
    /// <summary> Entity constructor for ORMs. </summary>
    protected EntityAuditableLogicalRemove() { }

    /// <summary> Constructor. </summary>
    protected EntityAuditableLogicalRemove(TId id) : base(id) { }

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
