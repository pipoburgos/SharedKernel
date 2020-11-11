using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    public class MsSqlEventBus : IEventBus
    {
        private readonly DbContext _context;

        public MsSqlEventBus(DbContext eventContext)
        {
            _context = eventContext;
        }

        public async Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            await Task.WhenAll(events.Select(domainEvent => Publish(domainEvent, cancellationToken)));
            await _context.SaveChangesAsync(cancellationToken);
        }

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