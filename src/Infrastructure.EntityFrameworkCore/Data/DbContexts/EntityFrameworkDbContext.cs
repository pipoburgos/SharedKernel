using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;
using System.Data;
using System.Reflection;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

/// <summary> Shared kernel DbContext. </summary>
public abstract class EntityFrameworkDbContext : DbContext, IDbContextAsync
{
    #region Members

    private readonly Assembly _assemblyConfigurations;
    private readonly IAuditableService? _auditableService;
    private readonly IClassValidatorService? _classValidatorService;
    private readonly List<OriginalEntry> _originalEntries;

    #endregion

    #region Constructors

    /// <summary> Constructor. </summary>
    protected EntityFrameworkDbContext(DbContextOptions options, string schema, Assembly assemblyConfigurations,
        IJsonSerializer? jsonSerializer = default, IClassValidatorService? classValidatorService = default,
        IAuditableService? auditableService = default) : base(options)
    {
        Id = Guid.NewGuid();
        _assemblyConfigurations = assemblyConfigurations;
        _classValidatorService = classValidatorService;
        _auditableService = auditableService;
        Schema = schema;
        JsonSerializer = jsonSerializer;
        // ReSharper disable once VirtualMemberCallInConstructor
        ChangeTracker.LazyLoadingEnabled = false;
        _originalEntries = new List<OriginalEntry>();
    }

    #endregion

    #region Properties

    /// <summary>  </summary>
    public Guid Id { get; }

    /// <summary>  </summary>
    public string Schema { get; }

    /// <summary>  </summary>
    public IJsonSerializer? JsonSerializer { get; }

    /// <summary>  </summary>
    public IDbConnection GetConnection => Database.GetDbConnection();

    #endregion

    /// <summary>  </summary>
    public void Add<TAggregateRoot, TId>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        base.Set<TAggregateRoot>().Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void Update<TAggregateRoot, TId>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        base.Set<TAggregateRoot>().Update(aggregateRoot);
    }

    /// <summary>  </summary>
    public void Remove<TAggregateRoot, TId>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        base.Set<TAggregateRoot>().Remove(aggregateRoot);
    }

    ///// <summary>  </summary>
    //protected virtual IQueryable<TAggregateRoot> GetQuery<TAggregateRoot, TId>(bool tracking = true,
    //    bool showDeleted = false, EntityFrameworkDbContext? dbContextBase = default) where TAggregateRoot : class
    //{
    //    IQueryable<TAggregateRoot> query = Set<TAggregateRoot>();

    //    query = GetAggregate<TAggregateRoot, TId>(query);

    //    if (typeof(IEntityIsTranslatable<>).IsAssignableFrom(typeof(TAggregateRoot)))
    //    {
    //        query = query
    //            .Cast<IEntityIsTranslatable<dynamic>>()
    //            .Include(a => a.Translations)
    //            .Cast<TAggregateRoot>();
    //    }

    //    if (!showDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(TAggregateRoot)))
    //    {
    //        query = query
    //            .Cast<IEntityAuditableLogicalRemove>()
    //            .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
    //            .Cast<TAggregateRoot>();
    //    }

    //    // ReSharper disable once ConvertIfStatementToReturnStatement
    //    if (tracking)
    //        return query;

    //    return query.AsNoTracking();
    //}

    ///// <summary>  </summary>
    //protected abstract IQueryable<TAggregateRoot> GetAggregate<TAggregateRoot, TId>(IQueryable<TAggregateRoot> query);

    /// <summary>  </summary>
    public TAggregateRoot GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class, IAggregateRoot<TId>
        where TId : notnull
    {
        throw new NotImplementedException();
    }

    /// <summary>  </summary>
    public override int SaveChanges()
    {
        try
        {
            _originalEntries.AddRange(ChangeTracker.Entries()
                .Select(e => new OriginalEntry(e, e.OriginalValues.Clone(), e.State)));

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
#else
        // ReSharper disable once RedundantCatchClause
        catch (Exception)
        {
            throw;
        }
#endif
    }

    /// <summary>  </summary>
    public virtual Result<int> SaveChangesResult()
    {
#if !NET462 && !NET47 && !NET471
        try
        {
#endif
        return Result
                .Create(Unit.Value)
#if NET47_OR_GREATER || NET6_0_OR_GREATER || NETSTANDARD
                .Combine(
                    _classValidatorService?.ValidateDataAnnotationsResult(ChangeTracker.Entries().Select(e => e.Entity)
                        .ToList()) ?? Result.Create(Unit.Value),
                    _classValidatorService?.ValidateValidatableObjectsResult(ChangeTracker.Entries()
                        .Select(e => e.Entity).OfType<IValidatableObject>().ToList()) ?? Result.Create(Unit.Value))
#endif
                .Tap(_ => _auditableService?.Audit(this))
                .Map(_ => base.SaveChanges());
#if !NET462 && !NET47 && !NET471
        }
        catch (DbUpdateException exUpdate)
        {
            return Result.Failure<int>(exUpdate.Entries.Select(e => Error.Create(e.ToString())));
        }
#endif
    }

    /// <summary>  </summary>
    public virtual Task<int> SaveChangesAsync()
    {
        return SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>  </summary>
    public new virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
#if !NET462 && !NET47 && !NET471
        try
        {
#endif
        _originalEntries.AddRange(ChangeTracker.Entries()
            .Select(e => new OriginalEntry(e, e.OriginalValues.Clone(), e.State)));

        _classValidatorService?.ValidateDataAnnotations(ChangeTracker.Entries().Select(e => e.Entity).ToList());
        _classValidatorService?.ValidateValidatableObjects(ChangeTracker.Entries().Select(e => e.Entity)
            .OfType<IValidatableObject>().ToList());
        _auditableService?.Audit(this);
        return await base.SaveChangesAsync(cancellationToken);
#if !NET462 && !NET47 && !NET471
        }
        catch (DbUpdateException dbUpdateException)
        {
            throw new Exception(string.Join(", ", dbUpdateException.Entries.Select(e => e.ToString())), dbUpdateException);
        }
#endif
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
                    Result.Failure<int>(Error.Create(expetion.InnerException?.ToString() ?? expetion.Message)));

    /// <summary>  </summary>
    public virtual int Rollback()
    {
        foreach (var entryInfo in _originalEntries)
        {
            var entry = entryInfo.Entry;

            switch (entryInfo.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Added;
                    break;
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entryInfo.OriginalValues);
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Deleted;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        _originalEntries.Clear();

        return SaveChanges();
    }

    /// <summary>  </summary>
    public virtual int RejectChanges()
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
    public virtual Result<int> RollbackResult()
    {
        return Rollback();
    }

    /// <inheritdoc />
    /// <summary> Rollback all changes. </summary>
    public virtual Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var entryInfo in _originalEntries)
        {
            var entry = entryInfo.Entry;

            switch (entryInfo.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Added;
                    break;
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entryInfo.OriginalValues);
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Deleted;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        _originalEntries.Clear();

        return SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(RollbackResult());
    }

    /// <summary>  </summary>
    public IQueryable<object> Set(Type type)
    {
        var x = GetType()
            .GetMethod("Set", Type.EmptyTypes)!
            .MakeGenericMethod(type);

        return (IQueryable<object>)x.Invoke(this, null)!;
    }

    /// <summary>  </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        modelBuilder.ApplyConfigurationsFromAssembly(_assemblyConfigurations);
    }

    /// <summary>  </summary>
    public Task AddAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        return base.Set<TAggregateRoot>().AddAsync(aggregateRoot, cancellationToken).AsTask();
    }

    /// <summary>  </summary>
    public Task UpdateAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        base.Set<TAggregateRoot>().Update(aggregateRoot);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task RemoveAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        base.Set<TAggregateRoot>().Remove(aggregateRoot);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        throw new NotImplementedException();
    }
}
