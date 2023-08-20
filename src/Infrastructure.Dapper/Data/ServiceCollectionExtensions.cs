using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;

namespace SharedKernel.Infrastructure.Dapper.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddDapperSqlServer(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, "Sql Server Dapper",
                HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

        services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));
        return services
            .AddTransient<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }

    /// <summary>  </summary>
    public static IServiceCollection AddDapperPostgreSql(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddHealthChecks()
            .AddNpgSql(connectionString, "SELECT 1;", null, "Postgre Dapper");

        services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));

        return services
            .AddTransient<IDbConnectionFactory>(_ => new PostgreSqlConnectionFactory(connectionString));
    }
}
