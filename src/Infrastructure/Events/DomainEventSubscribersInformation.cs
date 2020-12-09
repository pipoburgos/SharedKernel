using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Events
{
    public class DomainEventSubscribersInformation
    {
        private readonly Dictionary<Type, DomainEventSubscriberInformation> _information;

        public DomainEventSubscribersInformation(Dictionary<Type, DomainEventSubscriberInformation> information)
        {
            _information = information;
        }

        public Dictionary<Type, DomainEventSubscriberInformation>.ValueCollection All()
        {
            return _information.Values;
        }

        public List<string> GetAllEventsSubscribers()
        {
            return _information.Values.Select(x => x.SubscriberName()).ToList();
        }
    }
}