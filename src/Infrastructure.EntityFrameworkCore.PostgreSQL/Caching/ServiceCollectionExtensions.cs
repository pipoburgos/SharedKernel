using Community.Microsoft.Extensions.Caching.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Serializers;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.Caching;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// setup.ConnectionString = configuration["ConnectionString"];
    /// setup.SchemaName = configuration["SchemaName"];
    /// setup.TableName = configuration["TableName"];
    /// setup.DisableRemoveExpired = configuration["DisableRemoveExpired"]; // Optional - DisableRemoveExpired default is FALSE
    /// setup.CreateInfrastructure = configuration["CreateInfrastructure"]; // CreateInfrastructure is optional, default is TRUE This means que every time starts the application the creation of table and database functions will be verified.
    /// setup.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(30); //  ExpiredItemsDeletionInterval is optional This is the periodic interval to scan and delete expired items in the cache. Default is 30 minutes. Minimum allowed is 5 minutes. - If you need less than this please share your use case 😁, just for curiosity...
    /// </summary>
    public static IServiceCollection AddSharedKernelPostgreSqlDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        var postgreSqlCacheOptions = new PostgreSqlCacheOptions();
        configuration.GetSection(nameof(PostgreSqlCacheOptions)).Bind(postgreSqlCacheOptions);

        return services
            .AddOptions()
            .Configure<PostgreSqlCacheOptions>(configuration.GetSection(nameof(PostgreSqlCacheOptions)))
            .AddTransient<IBinarySerializer, BinarySerializer>()
            .AddTransient<ICacheHelper, DistributedCacheHelper>()
            .AddPostgreSqlHealthChecks(postgreSqlCacheOptions.ConnectionString, "PostgreSql DistributedCache", "DistributedCache")
            .AddDistributedPostgreSqlCache();

    }
}
