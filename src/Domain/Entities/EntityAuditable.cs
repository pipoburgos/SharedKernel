namespace SharedKernel.Domain.Entities;

/// <summary> An entity with creation and modification audit. </summary>
public abstract class EntityAuditable<TKey> : Entity<TKey>, IEntityAuditable where TKey : notnull
{
    /// <summary> Entity constructor for ORMs. </summary>
    protected EntityAuditable() { }

    /// <summary> Constructor. </summary>
    protected EntityAuditable(TKey id) : base(id) { }

    /// <summary> Constructor. </summary>
    /// <param name="id">Identifier</param>
    /// <param name="createdAt">Creation Date</param>
    /// <param name="createdBy">Creation user identifier</param>
    protected EntityAuditable(TKey id, DateTime createdAt, Guid createdBy) : base(id)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    /// <summary>  </summary>
    public Guid CreatedBy { get; private set; }

    /// <summary>  </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>  </summary>
    public Guid? LastModifiedBy { get; private set; }

    /// <summary>  </summary>
    public DateTime? LastModifiedAt { get; private set; }

    /// <summary>  </summary>
    public void Create(DateTime createdAt, Guid createdBy)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    /// <summary>  </summary>
    public void Change(DateTime lastModifiedAt, Guid lastModifiedBy)
    {
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
    }
}
