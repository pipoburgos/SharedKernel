namespace SharedKernel.Domain.Events
{
    using System.Collections.Generic;

    public class DomainEventPrimitive
    {
        public string Id { get; set; }
        public string AggregateId { get; set; }
        public string Name { get; set; }
        public string OccurredOn { get; set; }
        public Dictionary<string, string> Body { get; set; }
    }
}