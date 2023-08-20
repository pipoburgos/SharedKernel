using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.System.Threading;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register Redis IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddPostgreSqlSqlServerMutex(this IServiceCollection services, string connectionString)
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, "PostgreSql Mutex", "Mutex")
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory, PostgreSqlMutexFactory>();
    }
}
