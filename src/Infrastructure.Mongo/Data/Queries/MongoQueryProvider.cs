using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SharedKernel.Infrastructure.Mongo.Data.Queries;

/// <summary> Query provider for Mongo database. </summary>
public sealed class MongoQueryProvider
{
    private readonly IMongoDatabase _mongoDatabase;

    /// <summary> . </summary>
    public MongoQueryProvider(IOptions<MongoSettings> mongoSettings)
    {
        _mongoDatabase = new MongoClient(mongoSettings.Value.ConnectionString)
            .GetDatabase(mongoSettings.Value.Database);
    }

    /// <summary> </summary>
    public IQueryable<T> GetQuery<T>(AggregateOptions? options = default)
    {
        return _mongoDatabase.GetCollection<T>(typeof(T).Name).AsQueryable(options);
    }
}
