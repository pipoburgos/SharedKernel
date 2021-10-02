using System;
using System.Collections.Generic;
using System.Globalization;

namespace SharedKernel.Domain.Events
{
    /// <summary>
    /// Generic domain event
    /// </summary>
    public abstract class DomainEvent : IRequest
    {
        #region Constructors

        /// <summary>
        /// Domain Event serializable constructor
        /// </summary>
        protected DomainEvent() { }

        /// <summary>
        /// Domain event constructor
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="eventId"></param>
        /// <param name="occurredOn"></param>
        protected DomainEvent(string aggregateId, string eventId = null, string occurredOn = null)
        {
            AggregateId = aggregateId;
            EventId = eventId ?? Guid.NewGuid().ToString();
            OccurredOn = occurredOn ?? DateTime.Now.ToString("s", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The aggregate root identifier
        /// </summary>
        public string AggregateId { get; }

        /// <summary>
        /// The event identifier
        /// </summary>
        public string EventId { get; }

        /// <summary>
        /// When the event occurred
        /// </summary>
        public string OccurredOn { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The event identifier for message queues
        /// </summary>
        /// <returns></returns>
        public abstract string GetEventName();

        /// <summary>
        /// Create a new Domain event with default values
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="body"></param>
        /// <param name="eventId"></param>
        /// <param name="occurredOn"></param>
        /// <returns></returns>
        public abstract DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn);

        #endregion
    }
}