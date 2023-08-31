using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.FileSystem;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddFileSystemUnitOfWork<TInterface, TClass>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TClass : UnitOfWork, TInterface
        where TInterface : IUnitOfWork
    {
        services.Add(new ServiceDescriptor(typeof(UnitOfWork), typeof(UnitOfWork), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services;
    }

    /// <summary>  </summary>
    public static IServiceCollection AddFileSystemUnitOfWorkAsync<TInterface, TClass>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TClass : UnitOfWorkAsync, TInterface
        where TInterface : IUnitOfWorkAsync
    {
        services.Add(new ServiceDescriptor(typeof(UnitOfWorkAsync), typeof(UnitOfWorkAsync), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services.AddFileSystemUnitOfWork<TInterface, TClass>();
    }
}
