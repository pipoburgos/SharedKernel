using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
public static class RequestServiceExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddRequests<T>(this IServiceCollection services, Assembly assembly, string method,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        var commandRequests = GetRequests<T>(assembly);
        foreach (var eventType in commandRequests)
        {
            var eventName = GetUniqueName<T>(eventType, method);

            services.Add(new ServiceDescriptor(typeof(IRequestType),
                _ => new RequestType(eventName, eventType), serviceLifetime));
        }

        return services
            .AddTransient<IRequestSerializer, RequestSerializer>()
            .AddTransient<IRequestDeserializer, RequestDeserializer>()
            .AddTransient<IRequestMediator, RequestMediator>()
            .AddTransient<IRequestProviderFactory, RequestProviderFactory>();
    }

    private static string GetUniqueName<T>(Type type, string method)
    {
        var instance = ReflectionHelper.CreateInstance<T>(type);
        return type.GetMethod(method)?.Invoke(instance, null)?.ToString();
    }

    private static IEnumerable<Type> GetRequests<T>(Assembly domainAssembly)
    {
        return domainAssembly
            .GetTypes()
            .Where(p => typeof(T).IsAssignableFrom(p) && !p.IsAbstract);
    }
}
