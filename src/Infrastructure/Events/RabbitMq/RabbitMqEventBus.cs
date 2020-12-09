using RabbitMQ.Client.Exceptions;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// using SharedKernel.Infrastructure.Events.MsSql;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly DomainEventJsonSerializer _domainEventJsonSerializer;

        private readonly string _exchangeName;
        // private readonly MsSqlEventBus _failOverPublisher;

        public RabbitMqEventBus(
            ExecuteMiddlewaresService executeMiddlewaresService,
            RabbitMqPublisher rabbitMqPublisher,
            DomainEventJsonSerializer domainEventJsonSerializer,
            // MsSqlEventBus failOverPublisher,
            string exchangeName = "domain_events")
        {
            _executeMiddlewaresService = executeMiddlewaresService;
            _rabbitMqPublisher = rabbitMqPublisher;
            _domainEventJsonSerializer = domainEventJsonSerializer;
            // _failOverPublisher = failOverPublisher;
            _exchangeName = exchangeName;
        }

        public Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
        }

        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                _executeMiddlewaresService.Execute(@event);

                var serializedDomainEvent = _domainEventJsonSerializer.Serialize(@event);
                _rabbitMqPublisher.Publish(_exchangeName, @event.GetEventName(), serializedDomainEvent);
            }
            catch (RabbitMQClientException)
            {
                //await _failOverPublisher.Publish(new List<DomainEvent> {domainEvent}, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}