using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Mongo.Data.Queries;

namespace SharedKernel.Infrastructure.Mongo.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    /// <returns></returns>
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMongoHealthChecks(configuration)
            .AddTransient<MongoQueryProvider>();
    }
}
