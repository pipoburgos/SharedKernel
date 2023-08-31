using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

/// <summary>  </summary>
public class MongoUnitOfWork : UnitOfWork, IDisposable
{
    /// <summary>  </summary>
    protected readonly IClientSessionHandle Session;

    /// <summary>  </summary>
    protected readonly IMongoDatabase MongoDatabase;

    /// <summary>  </summary>
    protected readonly bool EnableTransactions;

    /// <summary>  </summary>
    public MongoUnitOfWork(IOptions<MongoSettings> options)
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
    public override int SaveChanges()
    {
        if (EnableTransactions)
            Session.StartTransaction();

        var total = base.SaveChanges();

        if (EnableTransactions)
            Session.CommitTransaction();

        return total;
    }

    /// <summary>  </summary>
    public void Dispose()
    {
        Session.Dispose();
    }
}
