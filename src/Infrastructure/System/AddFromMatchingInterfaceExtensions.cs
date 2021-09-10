using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public static class AddFromMatchingInterfaceExtensions
    {

        /// <summary>
        /// Register all classes that implement an interface equal to its name in transient mode
        /// Example: UserService -> IUserService.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IServiceCollection AddFromMatchingInterface(this IServiceCollection services, params Type[] types)
        {
            return services.AddFromMatchingInterface(default, types);
        }

        /// <summary>
        /// Register all classes that implement an interface equal to its name in transient mode
        /// Example: UserService -> IUserService.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="classesInclude"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        /// <exception cref="ActionNotSupportedException"></exception>
        public static IServiceCollection AddFromMatchingInterface(this IServiceCollection services, Func<Type, bool> classesInclude, params Type[] types)
        {
            classesInclude ??= n =>
                n.Name.EndsWith("Repository") ||
                n.Name.EndsWith("Service") ||
                n.Name.EndsWith("Queries") ||
                n.Name.EndsWith("ReadContext") ||
                n.Name.EndsWith("Manager") ||
                n.Name.EndsWith("Comparer");

            var classes = types
                .SelectMany(a => a.Assembly.GetTypes().Where(n => n.IsClass).Where(classesInclude))
                .ToList();

            foreach (var @class in classes)
            {
                services.AddTransient(@class);

                var interfaces = @class.FromMatchingInterface().ToList();

                switch (interfaces.Count)
                {
                    case > 1:
                        throw new ActionNotSupportedException($"Multiple inyecciones {@class.Name}");
                    case 1:
                        services.AddTransient(interfaces.Single(), @class);
                        break;
                }
            }

            return services;
        }

        private static IEnumerable<Type> FromMatchingInterface(this Type @class)
        {
            return @class.GetInterfaces().Where(n => n.Name == $"I{@class.Name}");
        }
    }
}