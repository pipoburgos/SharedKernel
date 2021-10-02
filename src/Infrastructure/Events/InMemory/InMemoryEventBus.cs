using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    internal class InMemoryEventBus : IEventBus
    {
        private readonly InMemoryDomainEventsConsumer _inMemoryConsumer;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;

        public InMemoryEventBus(
            InMemoryDomainEventsConsumer inMemoryConsumer,
            ExecuteMiddlewaresService executeMiddlewaresService)
        {
            _inMemoryConsumer = inMemoryConsumer;
            _executeMiddlewaresService = executeMiddlewaresService;
        }

        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return Publish(new List<DomainEvent> { @event }, cancellationToken);
        }

        public async Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            if (events == default)
                return;

            foreach (var domainEvent in events)
            {
                await _executeMiddlewaresService.ExecuteAsync(domainEvent, cancellationToken, (req, _) =>
                {
                    _inMemoryConsumer.Consume(req);
                    return Task.CompletedTask;
                });
            }
        }
    }
}