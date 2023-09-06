using AsyncKeyedLock;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.AsyncKeyedLock.System.Threading;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register in memory AsyncKeyedLock and IMutex factory. </summary>
    public static IServiceCollection AddAsyncKeyedLockMutex(this IServiceCollection services)
    {
        return services
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory, AsyncKeyedLockMutexFactory>()
            .AddSingleton(new AsyncKeyedLocker<string>(o =>
            {
                o.PoolSize = 20;
                o.PoolInitialFill = 1;
            }));
    }
}
