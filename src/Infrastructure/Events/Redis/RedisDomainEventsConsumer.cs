using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Redis
{
    /// <summary>
    /// Redis domain event consumer background service
    /// </summary>
    public class RedisDomainEventsConsumer : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDomainEventMediator _domainEventMediator;
        private readonly ICustomLogger<RedisDomainEventsConsumer> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public RedisDomainEventsConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();

            _connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            _domainEventMediator = scope.ServiceProvider.GetRequiredService<IDomainEventMediator>();
            _logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<RedisDomainEventsConsumer>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _connectionMultiplexer.GetSubscriber().SubscribeAsync(RedisChannel.Pattern("*"), (_, value) =>
            {
                try
                {
                    TaskHelper.RunSync(_domainEventMediator.ExecuteDomainSubscribers(value, stoppingToken));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                }
            });
        }
    }
}
