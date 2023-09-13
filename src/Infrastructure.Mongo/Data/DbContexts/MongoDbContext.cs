using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.Mongo.Data.DbContexts;

/// <summary>  </summary>
public class MongoDbContext : DbContextAsync, IDisposable
{
    /// <summary>  </summary>
    protected readonly IClientSessionHandle Session;

    /// <summary>  </summary>
    protected readonly IMongoDatabase MongoDatabase;

    /// <summary>  </summary>
    protected readonly bool EnableTransactions;

    /// <summary>  </summary>
    public MongoDbContext(IOptions<MongoSettings> options, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(auditableService, classValidatorService)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        MongoDatabase = mongoClient.GetDatabase(options.Value.Database);
        Session = mongoClient.StartSession();
        EnableTransactions = options.Value.EnableTransactions;
    }

    /// <summary>  </summary>
    public IMongoCollection<TAggregateRoot> Set<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
    {
        return MongoDatabase.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
    }

    /// <summary>  </summary>
    private IClientSessionHandle GetSession()
    {
        return Session;
    }

    /// <summary>  </summary>
    protected override void BeforeCommit()
    {
        if (EnableTransactions)
            Session.StartTransaction();
    }

    /// <summary>  </summary>
    protected override void AfterCommit()
    {
        if (EnableTransactions)
            Session.CommitTransaction();
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    {
        var aggregateRoot = Set<TAggregateRoot>().Find(a => a.Id!.Equals(id)).SingleOrDefault();

        if (aggregateRoot is IEntityAuditableLogicalRemove ag)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(ag)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    protected override void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        Set<TAggregateRoot>().InsertOne(GetSession(), aggregateRoot);
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        Set<TAggregateRoot>()
            .FindOneAndReplace(GetSession(), a => a.Id!.Equals(aggregateRoot.Id), aggregateRoot);
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        Set<TAggregateRoot>().DeleteOne(GetSession(), a => a.Id!.Equals(aggregateRoot.Id));
    }

    /// <summary>  </summary>
    protected override Task AddMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        return Set<T>().InsertOneAsync(GetSession(), aggregateRoot, cancellationToken: cancellationToken);
    }

    /// <summary>  </summary>
    protected override Task UpdateMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        return Set<T>().ReplaceOneAsync(GetSession(), a => a.Id!.Equals(aggregateRoot.Id), aggregateRoot,
            cancellationToken: cancellationToken);
    }

    /// <summary>  </summary>
    protected override Task DeleteMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        return Set<T>().DeleteOneAsync(GetSession(), a => a.Id!.Equals(aggregateRoot.Id),
            cancellationToken: cancellationToken);
    }

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id, CancellationToken cancellationToken) where TAggregateRoot : class
    {
        var cursor = await Set<TAggregateRoot>()
            .FindAsync(a => a.Id!.Equals(id), cancellationToken: cancellationToken);

        var aggregateRoot = cursor.SingleOrDefault();

        if (aggregateRoot is IEntityAuditableLogicalRemove ag)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(ag)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    protected override Task BeforeCommitAsync(CancellationToken cancellationToken)
    {
        if (EnableTransactions)
            Session.StartTransaction();

        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    protected override Task AfterCommitAsync(CancellationToken cancellationToken)
    {
        return EnableTransactions ? Session.CommitTransactionAsync(cancellationToken) : Task.CompletedTask;
    }

    /// <summary>  </summary>
    public void Dispose()
    {
        Session.Dispose();
    }
}
