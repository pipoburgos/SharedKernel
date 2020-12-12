using MassTransit;
using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.MassTransit
{
    public class MassTransitEventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventBus(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return Publish(new List<DomainEvent> { @event }, cancellationToken);
        }

        public Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            return events == null
                ? Task.CompletedTask
                : Task.WhenAll(events.Select(@event => _publishEndpoint.Publish(@event, cancellationToken)));
        }
    }
}
