using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class DomainEventSubscriberInformationService
    {
        private static Dictionary<Type, DomainEventSubscriberInformation> _information;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        public static void Register(Dictionary<Type, DomainEventSubscriberInformation> information)
        {
            _information = information;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, DomainEventSubscriberInformation>.ValueCollection All()
        {
            return _information.Values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllEventsSubscribers()
        {
            return _information.Values.Select(x => x.SubscriberName()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static List<string> GetAllEventsSubscribers(DomainEvent @event)
        {
            var values = _information.Values
                .Where(e => e.SubscribedEvent == @event.GetType());

            return values
                .Select(x => x.SubscriberName())
                .ToList();
        }

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

                if (!information.ContainsKey(subscriberClass))
                    information.Add(subscriberClass, new DomainEventSubscriberInformation(subscriberClass, eventType));
            }

            _information = information;
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventsSubscribers(this IServiceCollection services,
            Type type)
        {
            return services.AddDomainEventsSubscribers(type.Assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
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
                    services.AddScoped(@interface, @class);
                    services.AddScoped(@class);
                }
            }

            return services;
        }
    }
}