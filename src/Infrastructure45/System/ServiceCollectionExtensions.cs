using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace SharedKernel.Infrastructure.System
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFromAssembly(this IServiceCollection services, Assembly assembly, params Type[] genericTypes)
        {
            var classTypes = assembly.GetTypes().Select(t => t.GetTypeInfo()).Where(t => t.IsClass);

            foreach (var type in classTypes)
            {
                var interfaces = type.ImplementedInterfaces
                    .Select(i => i.GetTypeInfo())
                    .Where(i => i.IsGenericType && genericTypes.Any(genericType => i.GetGenericTypeDefinition() == genericType))
                    .ToList();

                foreach (var handlerInterfaceType in interfaces)
                {
                    services.AddScoped(handlerInterfaceType.AsType(), type.AsType());
                }
            }

            return services;
        }
    }
}
