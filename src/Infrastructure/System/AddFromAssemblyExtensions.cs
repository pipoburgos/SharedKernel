using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public static class AddFromAssemblyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="genericTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddFromAssembly(this IServiceCollection services, Assembly assembly,
            ServiceLifetime serviceLifetime, params Type[] genericTypes)
        {
            var classTypes = assembly.GetTypes().Select(t => t.GetTypeInfo()).Where(t => t.IsClass);

            foreach (var type in classTypes)
            {
                var interfaces = type.ImplementedInterfaces
                    .Select(i => i.GetTypeInfo())
                    .Where(i => i.IsGenericType &&
                                genericTypes.Any(genericType => i.GetGenericTypeDefinition() == genericType))
                    .ToList();

                foreach (var handlerInterfaceType in interfaces)
                {
                    services.Add(new ServiceDescriptor(handlerInterfaceType.AsType(), type.AsType(), serviceLifetime));
                }
            }

            return services;
        }
    }
}
