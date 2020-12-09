using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Redis
{
    public class RedisConsumer : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly DomainEventJsonDeserializer _jsonSerializer;
        private readonly DomainEventSubscribersInformation _information;
        private readonly DomainEventMediator _domainEventMediator;

        public RedisConsumer(
            IConnectionMultiplexer connectionMultiplexer,
            DomainEventJsonDeserializer jsonSerializer,
            DomainEventSubscribersInformation information,
            DomainEventMediator domainEventMediator)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _jsonSerializer = jsonSerializer;
            _information = information;
            _domainEventMediator = domainEventMediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var redisSubscriber = _connectionMultiplexer.GetSubscriber();
            await redisSubscriber.SubscribeAsync("*", async (channel, value) =>
            {
                var @event = _jsonSerializer.Deserialize(value);

                await _domainEventMediator.ExecuteOn(@event, _information.GetAllEventsSubscribers(), stoppingToken);
            });
        }
    }
}
