using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SharedKernel.Application.System.Threading;
using SharedKernel.Infrastructure.Mongo.Data;

namespace SharedKernel.Infrastructure.Mongo.System.Threading;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register Mongo IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddMongoMutex(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = new MongoSettings();
        configuration.GetSection(nameof(MongoSettings)).Bind(mongoSettings);
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Database);
        database.CreateCollection("signals", new CreateCollectionOptions { Capped = true, MaxSize = 1_000 });

        return services
            .AddMongoHealthChecks(configuration, "Mongo Mutex")
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory, MongoMutexFactory>();
    }
}
