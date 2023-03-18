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
            OccurredOn = occurredOn ?? DateTime.UtcNow.ToString("s", CultureInfo.CurrentCulture);
        }

        /// <summary> Domain event constructor. </summary>
        protected DomainEvent(string aggregateId, string eventId = null, string occurredOn = null)
        {
            AggregateId = aggregateId;
            EventId = eventId ?? Guid.NewGuid().ToString();
            OccurredOn = occurredOn ?? DateTime.UtcNow.ToString("s", CultureInfo.CurrentCulture);
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="body"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TEnum GetEnumFromBody<TEnum>(Dictionary<string, string> body, string name) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(body[name]))
                return default;

            if (Enum.TryParse<TEnum>(body[name], out var result))
                return result;

            throw new Exception($"Error parsing {body[name]}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DateTime ConvertToDateTime(Dictionary<string, string> body, string name)
        {
            return ConvertToDateTime(body[name]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime ConvertToDateTime(string value)
        {
            return Convert.ToDateTime(value);
        }

        #endregion
    }
}