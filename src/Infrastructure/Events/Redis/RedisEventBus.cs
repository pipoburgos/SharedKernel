using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Redis
{
    public class RedisEventBus : IEventBus
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly DomainEventJsonSerializer _domainEventJsonSerializer;

        public RedisEventBus(
            IConnectionMultiplexer connectionMultiplexer,
            ExecuteMiddlewaresService executeMiddlewaresService,
            DomainEventJsonSerializer domainEventJsonSerializer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _executeMiddlewaresService = executeMiddlewaresService;
            _domainEventJsonSerializer = domainEventJsonSerializer;
        }

        public Task Publish(List<DomainEvent> events, CancellationToken cancellationToken)
        {
            return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
        }

        public async Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            await _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken);
            var eventAsString = _domainEventJsonSerializer.Serialize(@event);
            await _connectionMultiplexer.GetSubscriber().PublishAsync("*", eventAsString);
        }
    }
}
