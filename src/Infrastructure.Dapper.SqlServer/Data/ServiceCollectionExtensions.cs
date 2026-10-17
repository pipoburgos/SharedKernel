using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using SharedKernel.Infrastructure.Dapper.SqlServer.Data.ConnectionFactory;

namespace SharedKernel.Infrastructure.Dapper.SqlServer.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelDapperSqlServer(this IServiceCollection services, string connectionString, string? serviceKey = null)
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, serviceKey == null ? "Sql Server Dapper" : $"Sql Server Dapper {serviceKey}",
                HealthStatus.Unhealthy, ["DB", "Sql", "SqlServer"]);

        if (serviceKey == null)
            return services
                .AddTransient<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString))
                .AddTransient<IDapperQueryProvider>(sp => new SqlServerDapperQueryProvider(
                    sp.GetRequiredService<ICustomLogger<SqlServerDapperQueryProvider>>(),
                    sp.GetRequiredService<IDbConnectionFactory>()));

        return services
            .AddKeyedTransient<IDbConnectionFactory>(serviceKey, (_, _) => new SqlConnectionFactory(connectionString))
            .AddKeyedTransient<IDapperQueryProvider>(serviceKey, (sp, key) => new SqlServerDapperQueryProvider(
                sp.GetRequiredService<ICustomLogger<SqlServerDapperQueryProvider>>(),
                sp.GetRequiredKeyedService<IDbConnectionFactory>(key)));
    }
}
