using System.Collections.Generic;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Events;

namespace SharedKernel.Domain.Aggregates
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
        private List<DomainEvent> _domainEvents = new List<DomainEvent>();

        public List<DomainEvent> PullDomainEvents()
        {
            var events = _domainEvents;

            _domainEvents = new List<DomainEvent>();

            return events;
        }

        protected void Record(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}