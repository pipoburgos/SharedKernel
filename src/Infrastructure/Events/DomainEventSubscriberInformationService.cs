using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedKernel.Infrastructure.Events
{
    public static class DomainEventSubscriberInformationService
    {
        /// <summary>
        /// Call just before compiling service collections
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventSubscribers(this IServiceCollection services)
        {
            var information = new Dictionary<Type, DomainEventSubscriberInformation>();

            var subscribers = services.Where(s => s.ServiceType.IsAssignableFrom(typeof(DomainEventSubscriber<>))).ToList();

            foreach (var subscriber in subscribers)
            {
                var subscriberClass = subscriber.ImplementationType;

                if (subscriberClass is null)
                    continue;

                var eventType = subscriberClass.BaseType?.GenericTypeArguments.Single();

                information.Add(subscriberClass, new DomainEventSubscriberInformation(subscriberClass, eventType));
            }

            services.AddScoped(_ => new DomainEventSubscribersInformation(information));
            return services;
        }

        public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services,
            Type type)
        {
            return services.AddDomainEventsSubscribers(type.Assembly);
        }

        public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services,
            Assembly assembly)
        {
            var classTypes = assembly.GetTypes().Select(t => t.GetTypeInfo()).Where(t => t.IsClass);

            foreach (var type in classTypes)
            {
                var interfaces = type.ImplementedInterfaces
                    .Select(i => i.GetTypeInfo())
                    .Where(i => i.IsAssignableFrom(typeof(DomainEventSubscriber<>)))
                    .ToList();

                foreach (var handlerInterfaceType in interfaces)
                {
                    var @interface = handlerInterfaceType.AsType();
                    var @class = type.AsType();
                    services.AddScoped( @interface, @class);
                    services.AddScoped(@class);
                }
            }

            return services;
        }
    }
}