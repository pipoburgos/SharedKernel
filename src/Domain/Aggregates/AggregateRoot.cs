using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Events;
using System.Collections.Generic;

namespace SharedKernel.Domain.Aggregates
{
    /// <summary>
    /// This root aggregate contains your domain identifier and events
    /// </summary>
    /// <typeparam name="TKey">The data type of the identifier</typeparam>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
        #region Constructors

        /// <summary>
        /// Aggregate Root constructor for ORMs
        /// </summary>
        protected AggregateRoot()
        {
        }

        /// <summary>
        /// Aggregate Root constructor
        /// </summary>
        /// <param name="id">The identifier</param>
        protected AggregateRoot(TKey id)
        {
            Id = id;
        }

        #endregion

        private List<DomainEvent> _domainEvents = new List<DomainEvent>();

        /// <summary>
        /// Extracts the domain events that have the root aggregate
        /// </summary>
        /// <returns>All domain events recordered</returns>
        public List<DomainEvent> PullDomainEvents()
        {
            var events = _domainEvents;
            _domainEvents = new List<DomainEvent>();
            events.ForEach(e => e.SetAggregateId(Id.ToString()));
            return events;
        }

        /// <summary>
        /// Stores a domain event for later extraction
        /// </summary>
        /// <param name="domainEvent"></param>
        public void Record(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}