using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;

namespace SharedKernel.Infrastructure.Events
{
    public static class DomainEventSubscriberInformationService
    {
        /// <summary>
        /// LLamar siempre al final ya que busca en el contenedor
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventSubscribersInformation(this IServiceCollection services)
        {
            var information = new Dictionary<Type, DomainEventSubscriberInformation>();

            var subscribers = services.Where(s => s.ServiceType.IsAssignableFrom(typeof(DomainEventSubscriber<>))).ToList();

            foreach (var subscriber in subscribers)
            {
                var subscriberClass = subscriber.ImplementationType;

                var eventType = subscriberClass.BaseType?.GenericTypeArguments.Single();

                information.Add(subscriberClass, new DomainEventSubscriberInformation(subscriberClass, eventType));
            }

            services.AddScoped(s => new DomainEventSubscribersInformation(information));
            return services;
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
                    var inter = handlerInterfaceType.AsType();
                    var clas = type.AsType();
                    services.AddScoped(inter, clas);
                    services.AddScoped(clas);
                }
            }

            return services;
        }

    }
}