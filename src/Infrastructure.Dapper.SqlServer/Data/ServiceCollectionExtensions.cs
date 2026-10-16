using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using SharedKernel.Infrastructure.Dapper.SqlServer.Data.ConnectionFactory;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure.Dapper.SqlServer.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelDapperSqlServer(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, string? serviceKey = "default")
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, serviceKey == "default" ? "Sql Server Dapper" : $"Sql Server Dapper {serviceKey}",
                HealthStatus.Unhealthy, ["DB", "Sql", "SqlServer"]);

        return services
            .AddKeyedTransient<IDbConnectionFactory>(serviceKey, (_, _) => new SqlConnectionFactory(connectionString))
            .AddSharedKernelKeyed<IDapperQueryProvider>(serviceKey,
                (sp, key) =>
                    new SqlServerDapperQueryProvider(sp.GetRequiredService<ICustomLogger<SqlServerDapperQueryProvider>>(),
                        sp.GetRequiredKeyedService<IDbConnectionFactory>(key)), serviceLifetime);
    }
}
