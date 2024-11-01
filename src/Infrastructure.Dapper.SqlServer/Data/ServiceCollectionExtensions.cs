using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Dapper.Data;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.SqlServer.Data.ConnectionFactory;

namespace SharedKernel.Infrastructure.Dapper.SqlServer.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddDapperSqlServer(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, string? serviceKey = "default")
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, "Sql Server Dapper",
                HealthStatus.Unhealthy, ["DB", "Sql", "SqlServer"]);

        return services.AddDapperQueryProvider(serviceLifetime, serviceKey)
            .AddKeyedTransient<IDbConnectionFactory>(serviceKey, (_, _) => new SqlConnectionFactory(connectionString));
    }
}
