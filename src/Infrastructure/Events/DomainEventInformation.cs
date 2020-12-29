using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class DomainEventsInformation
    {
        private static readonly Dictionary<string, Type> IndexedDomainEvents = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainAssembly"></param>
        public static void Register(Assembly domainAssembly)
        {
            var domainTypes = GetDomainTypes(domainAssembly);
            foreach (var eventType in domainTypes)
            {
                var eventName = GetEventName(eventType);
                if (!IndexedDomainEvents.ContainsKey(eventName))
                    IndexedDomainEvents.Add(eventName, eventType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type ForName(string name)
        {
            IndexedDomainEvents.TryGetValue(name, out var value);
            return value;
        }

        private static string GetEventName(Type eventType)
        {
            var instance = ReflectionHelper.CreateInstance<DomainEvent>(eventType);
            return eventType.GetMethod(nameof(DomainEvent.GetEventName))?.Invoke(instance, null)?.ToString();
        }

        private static IEnumerable<Type> GetDomainTypes(Assembly domainAssembly)
        {
            return domainAssembly
                .GetTypes()
                .Where(p => typeof(DomainEvent).IsAssignableFrom(p) && !p.IsAbstract);
        }
    }
}