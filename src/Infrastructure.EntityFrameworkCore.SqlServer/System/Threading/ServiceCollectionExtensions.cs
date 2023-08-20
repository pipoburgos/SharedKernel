using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.System.Threading;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register Redis IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddSqlServerMutex(this IServiceCollection services, string connectionString)
    {
        return services
            .AddSqlServerHealthChecks(connectionString, "SqlServer Mutex", "Mutex")
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory, SqlServerMutexFactory>();
    }
}
