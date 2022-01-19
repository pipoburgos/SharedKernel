using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;
using SharedKernel.Infrastructure.System.Threading.InMemory;
using SharedKernel.Infrastructure.System.Threading.SqlServerMutex;

namespace SharedKernel.Infrastructure.System.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class MutexServiceExtensions
    {
        /// <summary>
        /// Register in memory IMutexManager and IMutex factory
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryMutex(this IServiceCollection services)
        {
            return services
                .AddTransient<IMutexManager, MutexManager>()
                .AddSingleton<IMutexFactory, InMemoryMutexFactory>();
        }

        /// <summary>
        /// Register in memory IMutexManager and IMutex factory
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlServerMutex(this IServiceCollection services, string connectionString)
        {
            return services
                .AddTransient<IMutexManager, MutexManager>()
                .AddTransient<IMutexFactory>(_ => new SqlServerMutexFactory(connectionString));
        }
    }
}
