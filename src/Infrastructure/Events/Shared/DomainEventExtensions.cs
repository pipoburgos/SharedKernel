using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Events.Shared
{
    /// <summary>  </summary>
    internal static class DomainEventExtensions
    {
        /// <summary>  </summary>
        public static Dictionary<string, string> ToPrimitives(this DomainEvent domainEvent)
        {
            var primitives = new Dictionary<string, string>();

            domainEvent
                .GetType()
                .GetProperties()
                .Where(p =>
                    p.Name != nameof(DomainEvent.AggregateId) &&
                    p.Name != nameof(DomainEvent.EventId) &&
                    p.Name != nameof(DomainEvent.OccurredOn))
                .ToList()
                .ForEach(p => primitives.Add(p.Name, p.GetStringValue(domainEvent.GetType(), domainEvent)));

            return primitives;
        }
    }
}
