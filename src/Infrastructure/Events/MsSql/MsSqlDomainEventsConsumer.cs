using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.Shared.RegisterDomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    internal class MsSqlDomainEventsConsumer
    {
        private readonly DbContext _context;
        private readonly IDomainEventProviderFactory _domainEventProviderFactory;
        private readonly InMemoryEventBus _bus;
        private const int Chunk = 200;

        public MsSqlDomainEventsConsumer(
            InMemoryEventBus bus,
            DbContext context,
            IDomainEventProviderFactory domainEventProviderFactory)
        {
            _bus = bus;
            _context = context;
            _domainEventProviderFactory = domainEventProviderFactory;
        }

        public async Task Consume(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_context
                .Set<DomainEventPrimitive>()
                .Take(Chunk)
                .ToList()
                .Select(domainEvent => ExecuteSubscribersAsync(domainEvent, cancellationToken)));

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ExecuteSubscribersAsync(DomainEventPrimitive domainEventPrimitive, CancellationToken cancellationToken)
        {
            var domainEventType = _domainEventProviderFactory.Get(domainEventPrimitive.Name);

            var instance = (DomainEvent)Activator.CreateInstance(domainEventType);

            var result = (DomainEvent)domainEventType
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(DomainEvent.FromPrimitives))
                ?.Invoke(instance, new object[]
                {
                    domainEventPrimitive.AggregateId,
                    domainEventPrimitive.Body,
                    domainEventPrimitive.Id,
                    domainEventPrimitive.OccurredOn
                });

            await _bus.Publish(new List<DomainEvent> { result }, cancellationToken);

            _context.Set<DomainEventPrimitive>().Remove(domainEventPrimitive);

        }
    }
}