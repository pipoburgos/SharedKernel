using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

/// <summary>  </summary>
public class MongoUnitOfWorkAsync : UnitOfWorkAsync, IDisposable
{
    /// <summary>  </summary>
    protected readonly IClientSessionHandle Session;

    /// <summary>  </summary>
    protected readonly IMongoDatabase MongoDatabase;

    /// <summary>  </summary>
    protected readonly bool EnableTransactions;

    /// <summary>  </summary>
    public MongoUnitOfWorkAsync(IOptions<MongoSettings> options, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(auditableService, classValidatorService)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        MongoDatabase = mongoClient.GetDatabase(options.Value.Database);
        Session = mongoClient.StartSession();
        EnableTransactions = options.Value.EnableTransactions;
    }

    /// <summary>  </summary>
    public IMongoCollection<TAggregateRoot> GetCollection<TAggregateRoot>()
    {
        return MongoDatabase.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
    }

    /// <summary>  </summary>
    public IClientSessionHandle GetSession()
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
