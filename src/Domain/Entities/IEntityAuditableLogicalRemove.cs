namespace SharedKernel.Domain.Entities;

/// <summary>  </summary>
public interface IEntityAuditableLogicalRemove : IEntityAuditable
{
    /// <summary>  </summary>
    Guid? DeletedBy { get; }

    /// <summary>  </summary>
    DateTime? DeletedAt { get; }

    /// <summary>  </summary>
    void Delete(DateTime deletedAt, Guid userId);

    /// <summary>  </summary>
    void Restore();
}
