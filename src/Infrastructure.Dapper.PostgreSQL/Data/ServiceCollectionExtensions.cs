using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Dapper.Data;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
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
            .AddSharedKernelDapperQueryProvider(serviceLifetime, serviceKey)
            .AddTransient<IDbConnectionFactory>(_ => new PostgreSqlConnectionFactory(connectionString));
    }
}
