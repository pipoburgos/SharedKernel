using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using SharedKernel.Infrastructure.Dapper.PostgreSql.Data;
using SharedKernel.Infrastructure.Dapper.PostgreSQL.Data.ConnectionFactory;

namespace SharedKernel.Infrastructure.Dapper.PostgreSQL.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelDapperPostgreSql(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, string? serviceKey = "default")
    {
        services.AddHealthChecks()
            .AddNpgSql(connectionString, "SELECT 1;", null, "Postgre Dapper");

        return services
            .AddKeyedTransient<IDbConnectionFactory>(serviceKey, (_, _) => new PostgreSqlConnectionFactory(connectionString))
            .AddKeyedTransient<IDapperQueryProvider>(serviceKey, (sp, key) => new PostgreSqlDapperQueryProvider(
                sp.GetRequiredService<ICustomLogger<PostgreSqlDapperQueryProvider>>(),
                sp.GetRequiredKeyedService<IDbConnectionFactory>(key)));
    }
}
