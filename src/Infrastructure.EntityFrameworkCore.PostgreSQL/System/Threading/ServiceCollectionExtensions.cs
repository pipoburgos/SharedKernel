using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.System.Threading;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register PostgreSql IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddPostgreSqlMutex(this IServiceCollection services, string connectionString)
    {
        return services
            .AddPostgreSqlHealthChecks(connectionString, "PostgreSql Mutex", "Mutex")
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory>(_ => new PostgreSqlMutexFactory(connectionString));
    }
}
