using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;

namespace SharedKernel.Infrastructure.Dapper.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddDapperSqlServer(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)!;

        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, "Sql Server Dapper",
                HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

        services.AddTransient<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));

        return services;
    }

    /// <summary>  </summary>
    public static IServiceCollection AddDapperPostgreSql(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)!;

        services.AddHealthChecks()
            .AddNpgSql(connectionString, "SELECT 1;", null, "Postgre Dapper");

        services.AddTransient<IDbConnectionFactory>(_ => new PostgreSqlConnectionFactory(connectionString));
        services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));

        return services;
    }
}
