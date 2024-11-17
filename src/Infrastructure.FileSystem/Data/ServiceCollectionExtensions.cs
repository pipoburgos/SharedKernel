using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.FileSystem.Data.DbContexts;

namespace SharedKernel.Infrastructure.FileSystem.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelFileSystemDbContext<TDbContext>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : FileSystemDbContext
    {
        return services.AddSharedKernelDbContext<TDbContext>(serviceLifetime);
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelFileSystemUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : FileSystemDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelFileSystemDbContext<TDbContext>(serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
