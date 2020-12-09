using RabbitMQ.Client.Exceptions;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

// using SharedKernel.Infrastructure.Events.MsSql;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqEventBus : IEventBus
    {
        // private readonly MsSqlEventBus _failOverPublisher;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly DomainEventJsonSerializer _domainEventJsonSerializer;
        private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;

        public RabbitMqEventBus(
            // MsSqlEventBus failOverPublisher,
            ExecuteMiddlewaresService executeMiddlewaresService,
            RabbitMqPublisher rabbitMqPublisher,
            DomainEventJsonSerializer domainEventJsonSerializer,
            IOptions<RabbitMqConfigParams> rabbitMqParams)
        {
            // _failOverPublisher = failOverPublisher;
            _executeMiddlewaresService = executeMiddlewaresService;
            _rabbitMqPublisher = rabbitMqPublisher;
            _domainEventJsonSerializer = domainEventJsonSerializer;
            _rabbitMqParams = rabbitMqParams;
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
                _rabbitMqPublisher.Publish(_rabbitMqParams.Value.ExchangeName, @event.GetEventName(), serializedDomainEvent);
            }
            catch (RabbitMQClientException)
            {
                //await _failOverPublisher.Publish(new List<DomainEvent> {domainEvent}, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}