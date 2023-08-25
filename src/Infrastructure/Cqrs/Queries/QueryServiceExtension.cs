using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Infrastructure.Cqrs.Queries.InMemory;
using SharedKernel.Infrastructure.Requests.Middlewares;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Cqrs.Queries;

/// <summary>  </summary>
public static class QueryServiceExtension
{
    /// <summary>  </summary>
    public static IServiceCollection AddQueriesHandlers(this IServiceCollection services,
        Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.AddFromAssembly(assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));
        return services;
    }

    /// <summary>  </summary>
    public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Type type,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.AddFromAssembly(type.Assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

        return services;
    }

    /// <summary>  </summary>
    public static IServiceCollection AddQueriesHandlers(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] infrastructureAssembly)
    {
        foreach (var assembly in infrastructureAssembly)
            services.AddFromAssembly(assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

        return services;
    }

    /// <summary>  </summary>
    public static IServiceCollection AddQueriesHandlers(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Type[] queryHandlerTypes)
    {
        foreach (var queryHandlerType in queryHandlerTypes)
            services.AddFromAssembly(queryHandlerType.Assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

        return services;
    }

    /// <summary>  </summary>
    public static IServiceCollection AddInMemoryQueryBus(this IServiceCollection services)
    {
        return services
            .AddQueryBus()
            .AddTransient<IQueryBus, InMemoryQueryBus>();
    }

    /// <summary>  </summary>
    private static IServiceCollection AddQueryBus(this IServiceCollection services)
    {
        return services
            .AddTransient<IExecuteMiddlewaresService, ExecuteMiddlewaresService>();
    }
}
