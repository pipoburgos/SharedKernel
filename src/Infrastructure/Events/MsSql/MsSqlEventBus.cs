using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    /// <summary>
    /// 
    /// </summary>
    public class MsSqlEventBus : IEventBus
    {
        private readonly DbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventContext"></param>
        public MsSqlEventBus(DbContext eventContext)
        {
            _context = eventContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            await Task.WhenAll(events.Select(domainEvent => Publish(domainEvent, cancellationToken)));
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var value = new DomainEventPrimitive
            {
                Id = domainEvent.EventId,
                AggregateId = domainEvent.AggregateId,
                Body = domainEvent.ToPrimitives(),
                Name = domainEvent.GetEventName(),
                OccurredOn = domainEvent.OccurredOn
            };

            return _context.Set<DomainEventPrimitive>().AddAsync(value, cancellationToken).AsTask();
        }
    }
}