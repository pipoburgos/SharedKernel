using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public static class AddFromMatchingInterfaceExtensions
    {
        /// <summary>
        /// Register all clases thats implements interface
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IServiceCollection AddFromMatchingInterface<TInterface>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Type[] types)
        {
            if (types == default)
                return services;

            return services.AddFromMatchingInterface<TInterface>(serviceLifetime,
                types.Select(t => t.Assembly).ToArray());
        }

        /// <summary>
        /// Register all clases thats implements interface
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddFromMatchingInterface<TInterface>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] assemblies)
        {
            if (assemblies == default)
                return services;

            assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(TInterface).IsAssignableFrom(t))
                .ToList()
                .ForEach(type => services.Add(new ServiceDescriptor(typeof(TInterface), type, serviceLifetime)));

            return services;
        }

        /// <summary>
        /// Register all classes that implement an interface equal to its name in transient mode
        /// Example: UserService -> IUserService.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IServiceCollection AddFromMatchingInterface(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Type[] types)
        {
            return services.AddFromMatchingInterface(default, serviceLifetime, types);
        }

        /// <summary>
        /// Register all classes that implement an interface equal to its name in transient mode
        /// Example: UserService -> IUserService.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="classesInclude"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        /// <exception cref="ActionNotSupportedException"></exception>
        public static IServiceCollection AddFromMatchingInterface(this IServiceCollection services,
            Func<Type, bool> classesInclude, ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
            params Type[] types)
        {
            classesInclude ??= n =>
                n.Name.EndsWith("Repository") ||
                n.Name.EndsWith("Service") ||
                n.Name.EndsWith("Queries") ||
                n.Name.EndsWith("ReadContext") ||
                n.Name.EndsWith("Manager") ||
                n.Name.EndsWith("Comparer");

            var classes = types
                .SelectMany(a => a.Assembly.GetTypes().Where(type => type.IsClass).Where(classesInclude))
                .ToList();

            foreach (var @class in classes)
            {
                services.AddTransient(@class);

                var interfaces = @class.FromMatchingInterface().ToList();

                switch (interfaces.Count)
                {
                    case > 1:
                        throw new ActionNotSupportedException($"multiple injections for the class {@class.Name}");
                    case 1:
                        services.Add(new ServiceDescriptor(interfaces.Single(), @class, serviceLifetime));
                        break;
                }
            }

            return services;
        }

        private static IEnumerable<Type> FromMatchingInterface(this Type @class)
        {
            return @class.GetInterfaces().Where(n => @class.Name.EndsWith(n.Name[1..]));
        }
    }
}