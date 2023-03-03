using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    /// <summary>
    /// 
    /// </summary>
    public class MsSqlEventBus : IEventBus
    {
        private readonly IDomainEventJsonSerializer _domainEventJsonSerializer;
        private readonly DbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEventJsonSerializer"></param>
        /// <param name="eventContext"></param>
        public MsSqlEventBus(
            IDomainEventJsonSerializer domainEventJsonSerializer,
            DbContext eventContext)
        {
            _domainEventJsonSerializer = domainEventJsonSerializer;
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
                Body = _domainEventJsonSerializer.Serialize(domainEvent),
                Name = domainEvent.GetEventName(),
                OccurredOn = domainEvent.OccurredOn
            };

            return _context.Set<DomainEventPrimitive>().AddAsync(value, cancellationToken).AsTask();
        }
    }
}