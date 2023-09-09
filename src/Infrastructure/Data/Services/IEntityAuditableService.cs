namespace SharedKernel.Infrastructure.Data.Services;

/// <summary>  </summary>
public interface IEntityAuditableService
{
    /// <summary>  </summary>
    void Audit(IEnumerable<IEntityAuditable> added, IEnumerable<IEntityAuditable> modified,
        IEnumerable<IEntityAuditableLogicalRemove> deleted);
}
