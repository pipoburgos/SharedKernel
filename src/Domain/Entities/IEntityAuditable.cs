namespace SharedKernel.Domain.Entities;

/// <summary>
/// https://stackoverflow.com/questions/26355486/entity-framework-6-audit-track-changes
/// </summary>
public interface IEntityAuditable
{
    //#if NET40_OR_GREATER
    /// <summary>  </summary>
    Guid CreatedBy { get; }

    /// <summary>  </summary>
    DateTime CreatedAt { get; }

    /// <summary>  </summary>
    Guid? LastModifiedBy { get; }

    /// <summary>  </summary>
    DateTime? LastModifiedAt { get; }

    /// <summary> Sets the creation auditable properties. </summary>
    void Create(DateTime createdAt, Guid createdBy);

    /// <summary> Sets the modification auditable properties. </summary>
    void Change(DateTime lastModifiedAt, Guid lastModifiedBy);

    //#else
    //    /// <summary>  </summary>
    //    public Guid CreatedBy { get; private set; }

    //    /// <summary>  </summary>
    //    public DateTime CreatedAt { get; protected set; }

    //    /// <summary>  </summary>
    //    public Guid? LastModifiedBy { get; protected set; }

    //    /// <summary>  </summary>
    //    public DateTime? LastModifiedAt { get; protected set; }

    //    /// <summary> Sets the creation auditable properties. </summary>
    //    public void Create(DateTime createdAt, Guid createdBy)
    //    {
    //        CreatedAt = createdAt;
    //        CreatedBy = createdBy;
    //    }

    //    /// <summary> Sets the modification auditable properties. </summary>
    //    public void Change(DateTime lastModifiedAt, Guid lastModifiedBy)
    //    {
    //        LastModifiedAt = lastModifiedAt;
    //        LastModifiedBy = lastModifiedBy;
    //    }
    //#endif
}
