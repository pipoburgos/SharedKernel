using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Infrastructure.Cqrs.Queries.InMemory;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Cqrs.Queries;

/// <summary> . </summary>
public static class QueryServiceExtension
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelQueriesHandlers(this IServiceCollection services,
        Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.AddSharedKernelFromAssembly(assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));
        return services;
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelQueriesHandlers(this IServiceCollection services, Type type,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.AddSharedKernelFromAssembly(type.Assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

        return services;
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelQueriesHandlers(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] infrastructureAssembly)
    {
        foreach (var assembly in infrastructureAssembly)
            services.AddSharedKernelFromAssembly(assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

        return services;
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelQueriesHandlers(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Type[] queryHandlerTypes)
    {
        foreach (var queryHandlerType in queryHandlerTypes)
            services.AddSharedKernelFromAssembly(queryHandlerType.Assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

        return services;
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelInMemoryQueryBus(this IServiceCollection services)
    {
        return services
            .AddTransient<IQueryBus, InMemoryQueryBus>();
    }
}
