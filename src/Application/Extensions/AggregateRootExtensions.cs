using System.Collections.Generic;
using System.Linq;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Extensions
{
    /// <summary>
    /// AggregateRoot extensions
    /// </summary>
    public static class AggregateRootExtensions
    {
        /// <summary>
        /// Pull all domain events
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aggregateRoots"></param>
        /// <returns></returns>
        public static IEnumerable<DomainEvent> PullDomainEvents<T>(this IEnumerable<AggregateRoot<T>> aggregateRoots)
        {
            return aggregateRoots.SelectMany(a => a.PullDomainEvents());
        }
    }
}
