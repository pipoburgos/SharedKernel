using SharedKernel.Domain.Events;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Infrastructure.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Events.Redis
{
    public class RedisEventBus : IEventBus
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly DomainEventJsonSerializer _jsonSerializer;

        public RedisEventBus(
            IConnectionMultiplexer connectionMultiplexer,
            ExecuteMiddlewaresService executeMiddlewaresService,
            DomainEventJsonSerializer jsonSerializer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _executeMiddlewaresService = executeMiddlewaresService;
            _jsonSerializer = jsonSerializer;
        }

        public Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
        }

        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            _executeMiddlewaresService.Execute(@event);

            var eventAsString = _jsonSerializer.Serialize(@event);
            return _connectionMultiplexer.GetSubscriber().PublishAsync("*", eventAsString);
        }
    }
}
