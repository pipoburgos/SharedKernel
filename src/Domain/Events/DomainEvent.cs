using System;
using System.Collections.Generic;
using System.Globalization;

namespace SharedKernel.Domain.Events
{
    /// <summary> Generic domain event. </summary>
    public abstract class DomainEvent : IRequest
    {
        #region Constructors

        /// <summary> Domain event serializable constructor. </summary>
        protected DomainEvent() { }

        /// <summary> Domain event constructor. </summary>
        protected DomainEvent(string eventId = null, string occurredOn = null)
        {
            EventId = eventId ?? Guid.NewGuid().ToString();
            OccurredOn = occurredOn ?? DateTime.Now.ToString("s", CultureInfo.CurrentCulture);
        }

        /// <summary> Domain event constructor. </summary>
        protected DomainEvent(string aggregateId, string eventId = null, string occurredOn = null)
        {
            AggregateId = aggregateId;
            EventId = eventId ?? Guid.NewGuid().ToString();
            OccurredOn = occurredOn ?? DateTime.Now.ToString("s", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Properties

        /// <summary> The aggregate root identifier. </summary>
        public string AggregateId { get; private set; }

        /// <summary> The event identifier. </summary>
        public string EventId { get; }

        /// <summary> When the event occurred. </summary>
        public string OccurredOn { get; }

        #endregion

        #region Methods

        /// <summary> Sets aggregate id. </summary>
        public void SetAggregateId(string id)
        {
            AggregateId = id;
        }

        /// <summary> The event identifier for message queues. </summary>
        public abstract string GetEventName();

        /// <summary> Create a new Domain event with default values. </summary>
        public abstract DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn);

        #endregion
    }
}