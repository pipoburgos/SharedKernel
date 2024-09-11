using SharedKernel.Application.Security;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.Data.Services;

/// <summary> . </summary>
public class EntityAuditableService : IEntityAuditableService
{
    private readonly IIdentityService? _identityService;
    private readonly IDateTime _dateTime;

    /// <summary> . </summary>
    public EntityAuditableService(IIdentityService? identityService, IDateTime dateTime)
    {
        _identityService = identityService;
        _dateTime = dateTime;
    }

    /// <summary> . </summary>
    public void Audit(IEnumerable<IEntityAuditable> added, IEnumerable<IEntityAuditable> modified,
        IEnumerable<IEntityAuditableLogicalRemove> deleted)
    {
        var now = _dateTime.UtcNow;
        var user = _identityService?.UserId ?? Guid.Empty;

        foreach (var addedEntity in added)
        {
            addedEntity.Create(now, user);
        }

        foreach (var modifiedEntity in modified)
        {
            modifiedEntity.Change(now, user);
        }

        foreach (var removedEntity in deleted)
        {
            removedEntity.Delete(now, user);
        }
    }
}
