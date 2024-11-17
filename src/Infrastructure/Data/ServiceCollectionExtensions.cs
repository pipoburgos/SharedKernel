using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Data.UnitOfWorks;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validator;

namespace SharedKernel.Infrastructure.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelDbContext<TDbContext>(this IServiceCollection services,
        ServiceLifetime serviceLifetime) where TDbContext : DbContextAsync
    {
        services.Add(new ServiceDescriptor(typeof(IDbContextAsync), sp => sp.GetRequiredService<TDbContext>(),
            serviceLifetime));

        services.Add(new ServiceDescriptor(typeof(TDbContext), typeof(TDbContext), serviceLifetime));
        return services
            .AddTransient<IIdentityService, DefaultIdentityService>()
            .AddTransient<IDateTime, MachineDateTime>()
            .AddTransient<IEntityAuditableService, EntityAuditableService>()
            .AddTransient<IClassValidatorService, ClassValidatorService>();
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelGlobalUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TDbContext : GlobalUnitOfWork, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWork
    {
        services.Add(new ServiceDescriptor(typeof(TUnitOfWork), typeof(TDbContext), serviceLifetime));
        return services;
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelGlobalUnitOfWorkAsync<TUnitOfWork, TDbContext>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TDbContext : GlobalUnitOfWorkAsync, TUnitOfWork
        where TUnitOfWork : class, IUnitOfWorkAsync
    {
        services.Add(new ServiceDescriptor(typeof(TUnitOfWork), typeof(TDbContext), serviceLifetime));
        return services;
    }
}
