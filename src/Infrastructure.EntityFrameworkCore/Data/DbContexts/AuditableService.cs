using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Domain.Entities;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

/// <summary>  </summary>
public class AuditableService : IAuditableService
{
    private readonly IEntityAuditableService _entityAuditableService;
    private readonly IGuid _guid;
    private readonly IDateTime _dateTime;
    private readonly ICustomLogger<AuditableService> _customLogger;

    /// <summary>  </summary>
    public AuditableService(IEntityAuditableService entityAuditableService, IGuid guid, IDateTime dateTime,
        ICustomLogger<AuditableService> customLogger)
    {
        _entityAuditableService = entityAuditableService;
        _guid = guid;
        _dateTime = dateTime;
        _customLogger = customLogger;
    }

    /// <summary>  </summary>
    public void Audit(DbContext dbContext)
    {
        ModifyAuditableEntities(dbContext);

        if (Exists<AuditChange>(dbContext))
            AddToAuditChangeDbSet(dbContext);
    }

    private static bool Exists<TEntity>(DbContext context) where TEntity : class
    {
        var metaData = context.Model.FindEntityType(typeof(TEntity));

        return metaData != null;
    }

    /// <summary> https://stackoverflow.com/questions/26355486/entity-framework-6-audit-track-changes </summary>
    private void ModifyAuditableEntities(DbContext context)
    {
        var auditedEntities = context.ChangeTracker.Entries<IEntityAuditable>().ToList();
        var auditedLogicalRemoveEntities = context.ChangeTracker.Entries<IEntityAuditableLogicalRemove>()
            .Where(p => p.State == EntityState.Deleted).ToList();

        _entityAuditableService.Audit(
            auditedEntities.Where(p => p.State == EntityState.Added).Select(e => e.Entity).ToList(),
            auditedEntities.Where(p => p.State == EntityState.Modified).Select(e => e.Entity).ToList(),
            auditedLogicalRemoveEntities.Select(e => e.Entity));

        foreach (var removed in auditedLogicalRemoveEntities)
        {
            _customLogger.Verbose($"Logically deleting the entity {removed.Entity.GetType()}");
            removed.State = EntityState.Modified;
        }
    }

    private void AddToAuditChangeDbSet(DbContext context)
    {
        var auditChangesDbSet = context.Set<AuditChange>();

        var auditChanges = new List<AuditChange>();

        foreach (var entity in context.ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted)
            .Select(p => p.Entity))
        {
            var propertyNames = new List<string>();

            PropertyValues originalValues = default!;
            PropertyValues currentValues = default!;

            if (context.Entry(entity).State == EntityState.Modified)
            {
                originalValues = context.Entry(entity).OriginalValues;
                propertyNames = originalValues.Properties.Select(p => p.Name).ToList();
            }

            if (context.Entry(entity).State != EntityState.Deleted)
            {
                currentValues = context.Entry(entity).CurrentValues;
                propertyNames = currentValues.Properties.Select(p => p.Name).ToList();
            }

            var id = (propertyNames.Any(pn => pn == "Id") ? "Id" : null) ?? (propertyNames.Any(pn => pn == "EntityId") ? "EntityId" : null);

            var registryId = Guid.Empty.ToString();
            if (id == null)
            {
                var exception = new Exception($"Property not found {string.Join(",", propertyNames)}");
                _customLogger.Error(exception, exception.Message);
            }
            else
            {
                if (originalValues != default!)
                    registryId = originalValues[id]?.ToString() ?? Guid.Empty.ToString();

                if (currentValues != default!)
                    registryId = currentValues[id]?.ToString() ?? Guid.Empty.ToString();
            }

            var state = (State)context.Entry(entity).State;

            if (state == State.Deleted)
            {
                var auditChange = AuditChange.Create(_guid.NewGuid(), registryId, entity.GetType().Name, "Deleted",
                    null, null, _dateTime.UtcNow, state);

                auditChanges.Add(auditChange);
                continue;
            }

            foreach (var propertyName in propertyNames)
            {
                object? original = default;
                if (originalValues != default!)
                    original = originalValues[propertyName];

                object? current = default;
                if (currentValues != default!)
                    current = currentValues[propertyName];

                if ((original == default && current == default) || Equals(original, current))
                    continue;

                var auditChange = AuditChange.Create(_guid.NewGuid(), registryId, entity.GetType().Name,
                    propertyName, original?.ToString(), current?.ToString(), _dateTime.UtcNow, state);

                auditChanges.Add(auditChange);
            }
        }

        auditChangesDbSet.AddRange(auditChanges);
    }
}
