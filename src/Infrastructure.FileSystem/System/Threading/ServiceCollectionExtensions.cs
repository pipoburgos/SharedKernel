using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.FileSystem.System.Threading;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register File System IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddFileSystemMutex(this IServiceCollection services, DirectoryInfo directoryInfo)
    {
        return services
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory>(_ => new FileSystemMutexFactory(directoryInfo));
    }
}
