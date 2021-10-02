using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditableService : IAuditableService
    {
        private readonly IIdentityService _identityService;
        private readonly IDateTime _dateTime;
        private readonly IGuid _guid;
        private readonly ICustomLogger<AuditableService> _customLogger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityService"></param>
        /// <param name="dateTime"></param>
        /// <param name="guid"></param>
        /// <param name="customLogger"></param>
        public AuditableService(IIdentityService identityService, IDateTime dateTime,
            IGuid guid, ICustomLogger<AuditableService> customLogger)
        {
            _identityService = identityService;
            _dateTime = dateTime;
            _guid = guid;
            _customLogger = customLogger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
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

        /// <summary>
        /// https://stackoverflow.com/questions/26355486/entity-framework-6-audit-track-changes
        /// </summary>
        /// <param name="context"></param>
        private void ModifyAuditableEntities(DbContext context)
        {
            var now = _dateTime.UtcNow;
            var user = _identityService?.UserId ?? Guid.Empty;
            var auditedEntities = context.ChangeTracker.Entries<IEntityAuditable>().ToList();

            foreach (var added in auditedEntities.Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity))
            {
                added.Create(now, user);
            }

            foreach (var modified in auditedEntities.Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity))
            {
                modified.Change(now, user);
            }

            var auditedLogicalRemoveEntities = context
                .ChangeTracker
                .Entries<IEntityAuditableLogicalRemove>()
                .Where(p => p.State == EntityState.Deleted)
                .ToList();

            foreach (var removed in auditedLogicalRemoveEntities)
            {
                _customLogger.Verbose($"Logically deleting the entity {removed.Entity.GetType()}");
                removed.Entity.Delete(now, user);
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

                PropertyValues originalValues = null;
                PropertyValues currentValues = null;

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
                    if (originalValues != null)
                        registryId = originalValues[id]?.ToString() ?? Guid.Empty.ToString();

                    if (currentValues != null)
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
                    object original = null;
                    if (originalValues != null)
                        original = originalValues[propertyName];

                    object current = null;
                    if (currentValues != null)
                        current = currentValues[propertyName];

                    if (original == default && current == default || Equals(original, current))
                        continue;

                    var auditChange = AuditChange.Create(_guid.NewGuid(), registryId, entity.GetType().Name,
                        propertyName, original?.ToString(), current?.ToString(), _dateTime.UtcNow, state);

                    auditChanges.Add(auditChange);
                }
            }

            auditChangesDbSet.AddRange(auditChanges);
        }
    }
}
