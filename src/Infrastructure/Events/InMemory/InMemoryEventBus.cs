using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    internal class InMemoryEventBus : IEventBus
    {
        private readonly DomainEventsToExecute _domainEventsToExecute;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;

        public InMemoryEventBus(
            DomainEventsToExecute domainEventsToExecute,
            ExecuteMiddlewaresService executeMiddlewaresService)
        {
            _domainEventsToExecute = domainEventsToExecute;
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
                await _executeMiddlewaresService.ExecuteAsync(domainEvent, cancellationToken, (@event, _) =>
                {
                    _domainEventsToExecute.Add(@event);
                    return Task.CompletedTask;
                });
            }
        }
    }
}