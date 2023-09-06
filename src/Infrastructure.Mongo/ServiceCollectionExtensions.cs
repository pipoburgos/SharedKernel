using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Mongo.Data;

namespace SharedKernel.Infrastructure.Mongo;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    internal static IServiceCollection AddMongoHealthChecks(this IServiceCollection services, IConfiguration configuration, string name)
    {
        var connectionString = configuration
            .GetSection($"{nameof(MongoSettings)}:{nameof(MongoSettings.ConnectionString)}").Value;

        services
            .AddMongoOptions(configuration)
            .AddHealthChecks()
            .AddMongoDb(connectionString!, name, HealthStatus.Unhealthy, new[] { "DB", "NoSql", "Mongo" });

        return services;
    }

    /// <summary>  </summary>
    internal static IServiceCollection AddMongoOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddOptions()
            .Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    }
}
