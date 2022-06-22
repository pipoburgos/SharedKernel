using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Aggregates;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    /// <summary>
    /// Shared kernel DbContext
    /// </summary>
    public class DbContextBase : DbContext, IQueryableUnitOfWork
    {
        #region Members

        private readonly Assembly _assemblyConfigurations;
        private readonly IAuditableService _auditableService;
        private readonly IValidatableObjectService _validatableObjectService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="schema"></param>
        /// <param name="assemblyConfigurations"></param>
        /// <param name="auditableService"></param>
        /// <param name="validatableObjectService"></param>
        public DbContextBase(DbContextOptions options, string schema, Assembly assemblyConfigurations,
            IValidatableObjectService validatableObjectService, IAuditableService auditableService) : base(options)
        {
            _assemblyConfigurations = assemblyConfigurations;
            _auditableService = auditableService;
            _validatableObjectService = validatableObjectService;
            Schema = schema;
            // ReSharper disable once VirtualMemberCallInConstructor
            ChangeTracker.LazyLoadingEnabled = false;
        }


        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Schema { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDbConnection GetConnection => Database.GetDbConnection();

        #endregion

        #region IUnitOfWorkAsync Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return RollbackAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            return SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                _validatableObjectService?.ValidateDomainEntities(this);
                _validatableObjectService?.Validate(this);
                _auditableService?.Audit(this);
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                await RollbackAsync(cancellationToken);
                throw;// new SharedKernelInfrastructureException(nameof(ExceptionCodes.EF_CORE_SAVE_CHANGES), ex);
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <returns></returns>
        public DbSet<TAggregateRoot> SetAggregate<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
        {
            return base.Set<TAggregateRoot>();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            modelBuilder.ApplyConfigurationsFromAssembly(_assemblyConfigurations);
        }

        #endregion
    }
}