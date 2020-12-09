using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly DomainEventSubscribersInformation _information;
        private readonly DomainEventMediator _domainEventMediator;

        public InMemoryEventBus(
            DomainEventSubscribersInformation information,
            DomainEventMediator domainEventMediator)
        {
            _information = information;
            _domainEventMediator = domainEventMediator;
        }

        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return Publish(new List<DomainEvent> { @event }, cancellationToken);
        }

        public async Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                await _domainEventMediator.ExecuteOn(@event, _information.GetAllEventsSubscribers(), cancellationToken);
            }
        }
    }
}