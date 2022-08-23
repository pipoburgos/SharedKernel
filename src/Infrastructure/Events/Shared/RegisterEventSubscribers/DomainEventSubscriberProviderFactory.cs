using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers
{
    internal class DomainEventSubscriberProviderFactory : IDomainEventSubscriberProviderFactory
    {
        private readonly List<IDomainEventSubscriberType> _infos;
        private readonly Dictionary<Type, IEnumerable<Type>> _dictionary;

        /// <summary>  </summary>
        public DomainEventSubscriberProviderFactory(IServiceProvider serviceProvider)
        {
            _infos = serviceProvider
                .GetServices<IDomainEventSubscriberType>()
                .ToList();

            _dictionary = _infos
                .GroupBy(e => e.SubscribedEvent)
                .ToDictionary(e => e.Key, c => c.Select(a => a.GetSubscriber()));
        }

        public IEnumerable<Type> GetSubscribers(DomainEvent @event)
        {
            return _dictionary.ContainsKey(@event.GetType()) ? _dictionary[@event.GetType()] : Enumerable.Empty<Type>();
        }

        public List<IDomainEventSubscriberType> GetAll()
        {
            return _infos;
        }
    }
}
