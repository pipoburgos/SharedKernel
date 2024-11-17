using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.System;

/// <summary> . </summary>
public static class KeyedServiceExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelKeyed<TService>(
        this IServiceCollection services,
        object? serviceKey,
        Func<IServiceProvider, object?, TService> implementationFactory,
        ServiceLifetime lifetime)
        where TService : class
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<TService>(sp => implementationFactory(sp, serviceKey));
                break;

            case ServiceLifetime.Scoped:
                services.AddScoped<TService>(sp => implementationFactory(sp, serviceKey));
                break;

            case ServiceLifetime.Transient:
                services.AddTransient<TService>(sp => implementationFactory(sp, serviceKey));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return services;
    }
}

