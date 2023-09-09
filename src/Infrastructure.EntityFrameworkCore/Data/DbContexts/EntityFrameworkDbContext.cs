using SharedKernel.Application.Validator;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.DbContexts;
using System.Data;
using System.Reflection;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

/// <summary> Shared kernel DbContext. </summary>
public abstract class EntityFrameworkDbContext : DbContext, IDbContext, IQueryableUnitOfWork
{
    #region Members

    private readonly Assembly _assemblyConfigurations;
    private readonly IAuditableService? _auditableService;
    private readonly IClassValidatorService? _classValidatorService;

    #endregion

    #region Constructors

    /// <summary> Constructor. </summary>
    protected EntityFrameworkDbContext(DbContextOptions options, string schema, Assembly assemblyConfigurations,
        IClassValidatorService? classValidatorService = default,
        IAuditableService? auditableService = default) : base(options)
    {
        _assemblyConfigurations = assemblyConfigurations;
        _classValidatorService = classValidatorService;
        _auditableService = auditableService;
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
    public void Add<T, TId>(T aggregateRoot) where T : class, IAggregateRoot<TId> where TId : notnull
    {
        Set<T>().Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void Update<T, TId>(T aggregateRoot, T originalAggregateRoot) where T : class, IAggregateRoot<TId> where TId : notnull
    {
        Set<T>().Update(aggregateRoot);
    }

    /// <summary>  </summary>
    public void Remove<T, TId>(T aggregateRoot, T originalAggregateRoot) where T : class, IAggregateRoot<TId> where TId : notnull
    {
        Set<T>().Remove(aggregateRoot);
    }

    /// <summary>  </summary>
    public override int SaveChanges()
    {
        try
        {
            _classValidatorService?.ValidateDataAnnotations(ChangeTracker.Entries().Select(e => e.Entity).ToList());
            _classValidatorService?.ValidateValidatableObjects(ChangeTracker.Entries().Select(e => e.Entity)
                .OfType<IValidatableObject>().ToList());
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
                    _classValidatorService?.ValidateDataAnnotationsResult(ChangeTracker.Entries().Select(e => e.Entity)
                        .ToList()) ?? Result.Create(Unit.Value),
                    _classValidatorService?.ValidateValidatableObjectsResult(ChangeTracker.Entries()
                        .Select(e => e.Entity).OfType<IValidatableObject>().ToList()) ?? Result.Create(Unit.Value))
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
            _classValidatorService?.ValidateDataAnnotations(ChangeTracker.Entries().Select(e => e.Entity).ToList());
            _classValidatorService?.ValidateValidatableObjects(ChangeTracker.Entries().Select(e => e.Entity)
                .OfType<IValidatableObject>().ToList());
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
            .Combine(
                _classValidatorService?.ValidateDataAnnotationsResult(ChangeTracker.Entries().Select(e => e.Entity)
                    .ToList()) ?? Result.Create(Unit.Value),
                _classValidatorService?.ValidateValidatableObjectsResult(ChangeTracker.Entries()
                    .Select(e => e.Entity).OfType<IValidatableObject>().ToList()) ?? Result.Create(Unit.Value))
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

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return Rollback();
    }

    /// <inheritdoc />
    /// <summary> Rollback all changes. </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Rollback());
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(RollbackResult());
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
