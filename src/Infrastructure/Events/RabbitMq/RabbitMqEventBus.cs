using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMqEventBus : IEventBus
    {
        private const string HeaderReDelivery = "redelivery_count";
        // private readonly MsSqlEventBus _failOverPublisher;
        private readonly IDomainEventJsonSerializer _domainEventJsonSerializer;
        private readonly RabbitMqConnectionFactory _config;
        private readonly IExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEventJsonSerializer"></param>
        /// <param name="config"></param>
        /// <param name="executeMiddlewaresService"></param>
        /// <param name="rabbitMqParams"></param>
        public RabbitMqEventBus(
            // MsSqlEventBus failOverPublisher,
            IDomainEventJsonSerializer domainEventJsonSerializer,
            RabbitMqConnectionFactory config,
            IExecuteMiddlewaresService executeMiddlewaresService,
            IOptions<RabbitMqConfigParams> rabbitMqParams)
        {
            // _failOverPublisher = failOverPublisher;
            _domainEventJsonSerializer = domainEventJsonSerializer;
            _config = config;
            _executeMiddlewaresService = executeMiddlewaresService;
            _rabbitMqParams = rabbitMqParams;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, (req, _) =>
            {
                try
                {
                    var serializedDomainEvent = _domainEventJsonSerializer.Serialize(req);

                    var channel = _config.Channel();
                    channel.ExchangeDeclare(_rabbitMqParams.Value.ExchangeName, ExchangeType.Topic);

                    var body = Encoding.UTF8.GetBytes(serializedDomainEvent);
                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object> {{HeaderReDelivery, 0}};

                    channel.BasicPublish(_rabbitMqParams.Value.ExchangeName, req.GetEventName(), properties, body);
                }
                catch (RabbitMQClientException)
                {
                    //await _failOverPublisher.Publish(new List<DomainEvent> {domainEvent}, cancellationToken);
                }

                return Task.CompletedTask;
            });
        }
    }
}