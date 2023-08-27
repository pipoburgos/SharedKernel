using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.RailwayOrientedProgramming;
using System.Data;
using System.Reflection;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

/// <summary> Shared kernel DbContext. </summary>
public class DbContextBase : DbContext, IQueryableUnitOfWork
{
    #region Members

    private readonly Assembly _assemblyConfigurations;
    private readonly IAuditableService? _auditableService;
    private readonly IValidatableObjectService? _validatableObjectService;

    #endregion

    #region Constructors

    /// <summary> Constructor. </summary>
    public DbContextBase(DbContextOptions options, string schema, Assembly assemblyConfigurations,
        IValidatableObjectService? validatableObjectService, IAuditableService? auditableService) : base(options)
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

    /// <summary>  </summary>
    public string Schema { get; }

    /// <summary>  </summary>
    public IDbConnection GetConnection => Database.GetDbConnection();

    #endregion

    #region IUnitOfWorkAsync Methods

    /// <summary>  </summary>
    public override int SaveChanges()
    {
        try
        {
            _validatableObjectService?.ValidateDomainEntities(this);
            _validatableObjectService?.Validate(this);
            _auditableService?.Audit(this);
            return base.SaveChanges();
        }
#if !NET462 && !NET47 && !NET471
        catch (DbUpdateException exUpdate)
        {
            throw new Exception(string.Join(", ", exUpdate.Entries.Select(e => e.ToString())), exUpdate);
        }
#endif
        finally
        {
            Rollback();
        }
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        try
        {
            return Result
                .Create(Unit.Value)
#if NET47_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD
                .Combine(
                    _validatableObjectService?.ValidateDomainEntitiesResult(this) ?? Result.Create(Unit.Value),
                    _validatableObjectService?.ValidateResul(this) ?? Result.Create(Unit.Value))
#endif
                .Tap(_ => _auditableService?.Audit(this))
                .Map(_ => base.SaveChanges());
        }
#if !NET462 && !NET47 && !NET471
        catch (DbUpdateException exUpdate)
        {
            return Result.Failure<int>(exUpdate.Entries.Select(e => Error.Create(e.ToString())));
        }
#endif
        finally
        {
            Rollback();
        }
    }

    /// <summary>  </summary>
    public Task<int> SaveChangesAsync()
    {
        return SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>  </summary>
    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            _validatableObjectService?.ValidateDomainEntities(this);
            _validatableObjectService?.Validate(this);
            _auditableService?.Audit(this);
            return await base.SaveChangesAsync(cancellationToken);
        }
#if !NET462 && !NET47 && !NET471
        catch (DbUpdateException dbUpdateException)
        {
            throw new Exception(string.Join(", ", dbUpdateException.Entries.Select(e => e.ToString())), dbUpdateException);
        }
#endif
        finally
        {
            await RollbackAsync(cancellationToken);
        }
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken) =>
        Result
            .Create(Unit.Value)
            .Combine(_validatableObjectService?.ValidateDomainEntitiesResult(this) ?? Result.Create(Unit.Value),
                _validatableObjectService?.ValidateResul(this) ?? Result.Create(Unit.Value))
            .Tap(_ => _auditableService?.Audit(this))
            .Map(_ => Unit.Value)
            .TryBind<Unit, int, DbUpdateException>(_ => base.SaveChangesAsync(cancellationToken),
                dbUpdateException => Result.Failure<int>(new List<Error>
                        {Error.Create(dbUpdateException.InnerException?.ToString() ?? dbUpdateException.Message)}
                    .Concat(dbUpdateException.Entries.Select(e => Error.Create(e.ToString())))),
                expetion =>
                    Result.Failure<int>(Error.Create(expetion.InnerException?.ToString() ?? expetion.Message)),
                () => RollbackAsync(cancellationToken));

    /// <summary>  </summary>
    public int Rollback()
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

        return changedEntries.Count;
    }

    /// <inheritdoc />
    /// <summary> Rollback all changes. </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Rollback());
    }

    #endregion

    #region IQueryableUnitOfWork Members

    /// <summary>  </summary>
    public DbSet<TAggregateRoot> SetAggregate<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
    {
        return base.Set<TAggregateRoot>();
    }

    /// <summary>  </summary>
    public IQueryable<object> Set(Type type)
    {
        var x = GetType()
            .GetMethod("Set", Type.EmptyTypes)!
            .MakeGenericMethod(type);

        return (IQueryable<object>)x.Invoke(this, null)!;
    }

    #endregion

    #region Protected Methods

    /// <summary>  </summary>
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
