using System.Collections.Generic;

namespace SharedKernel.Domain.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventPrimitive
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AggregateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OccurredOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Body { get; set; }
    }
}