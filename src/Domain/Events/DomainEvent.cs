using System;
using System.Collections.Generic;
using System.Globalization;

namespace SharedKernel.Domain.Events
{
    public class DomainEvent : IRequest
    {
        public string AggregateId { get; }
        public string EventId { get; }
        public string OccurredOn { get; }

        protected DomainEvent() { }

        protected DomainEvent(string aggregateId, string eventId = null, string occurredOn = null)
        {
            AggregateId = aggregateId;
            EventId = eventId ?? Guid.NewGuid().ToString();
            OccurredOn = occurredOn ?? DateTime.Now.ToString("s", CultureInfo.CurrentCulture);
        }

        public virtual string GetEventName() => string.Empty;

        public virtual DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
            string occurredOn) => new DomainEvent(aggregateId, eventId, occurredOn);

        protected DateTime ParseExact(string date)
        {
            return DateTime.Parse(date, new CultureInfo("es-ES"));
        }
    }
}