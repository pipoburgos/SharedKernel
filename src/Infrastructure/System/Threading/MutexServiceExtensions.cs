using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;
using SharedKernel.Infrastructure.System.Threading.InMemory;

namespace SharedKernel.Infrastructure.System.Threading;

/// <summary>  </summary>
public static class MutexServiceExtensions
{
    /// <summary> Register in memory IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddInMemoryMutex(this IServiceCollection services)
    {
        return services
            .AddTransient<IMutex, InMemoryMutex>()
            .AddTransient<IMutexManager, MutexManager>()
            .AddSingleton<IMutexFactory, InMemoryMutexFactory>();
    }
}
