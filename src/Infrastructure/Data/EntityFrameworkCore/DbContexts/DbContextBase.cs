using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.System;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    public class DbContextBase : DbContext, IQueryableUnitOfWork
    {
        #region Members

        private readonly Assembly _assemblyConfigurations;
        private readonly IAuditableService _auditableService;
        private readonly IEventBus _eventBus;

        #endregion

        #region Constructors

        public DbContextBase(DbContextOptions options, string schema, Assembly assemblyConfigurations,
            IAuditableService auditableService, IEventBus eventBus) : base(options)
        {
            _assemblyConfigurations = assemblyConfigurations;
            _auditableService = auditableService;
            _eventBus = eventBus;
            Schema = schema;
            // ReSharper disable once VirtualMemberCallInConstructor
            ChangeTracker.LazyLoadingEnabled = false;
        }


        #endregion

        #region Properties

        public string Schema { get; }

        public IDbConnection GetConnection => Database.GetDbConnection();

        #endregion

        #region IUnitOfWorkAsync Methods

        public int Rollback()
        {
            return RollbackAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                Validate();
                _auditableService?.Audit(this);
                await DispatchDomainEventsAsync(cancellationToken);
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                await RollbackAsync(cancellationToken);
                throw new SharedKernelInfrastructureException(nameof(ExceptionCodes.EF_CORE_SAVE_CHANGES), ex);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Rollback all changes
        /// </summary>
        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            var changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            return Task.FromResult(changedEntries.Count);
        }

        #endregion

        #region IQueryableUnitOfWork Members

        public DbSet<TAggregateRoot> SetAggregate<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
        {
            return base.Set<TAggregateRoot>();
        }

        #endregion

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            modelBuilder.ApplyConfigurationsFromAssembly(_assemblyConfigurations);
        }

        protected virtual void Validate()
        {
            var changedEntities = ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added ||
                            _.State == EntityState.Modified);

            var errors = new List<ValidationResult>(); // all errors are here
            foreach (var e in changedEntities)
            {
                var vc = new ValidationContext(e.Entity, null, null);
                Validator.TryValidateObject(e.Entity, vc, errors, true);
            }
        }

        private Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
        {
            var domainEvents = ChangeTracker
                .Entries<IAggregateRoot>()
                .SelectMany(x => x.Entity.PullDomainEvents())
                .ToList();

            return _eventBus != null ? _eventBus?.Publish(domainEvents, cancellationToken) : TaskHelper.CompletedTask;
        }

        #endregion
    }
}