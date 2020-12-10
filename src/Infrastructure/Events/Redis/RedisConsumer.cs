using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Redis
{
    public class RedisConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RedisConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var initialScope = _serviceScopeFactory.CreateScope();

            var redisSubscriber = initialScope.ServiceProvider
                .GetRequiredService<IConnectionMultiplexer>()
                .GetSubscriber();

            await redisSubscriber.SubscribeAsync("*", async (channel, value) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var @event = scope.ServiceProvider
                    .GetRequiredService<DomainEventJsonDeserializer>()
                    .Deserialize(value);

                await scope.ServiceProvider
                    .GetRequiredService<DomainEventMediator>()
                    .ExecuteOn(@event, scope.ServiceProvider
                        .GetRequiredService<DomainEventSubscribersInformation>()
                        .GetAllEventsSubscribers(), stoppingToken);
            });
        }
    }
}
