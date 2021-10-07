using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.System;
using StackExchange.Redis;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public RedisDomainEventsConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            _connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            _domainEventMediator = scope.ServiceProvider.GetRequiredService<IDomainEventMediator>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _connectionMultiplexer.GetSubscriber().SubscribeAsync("*", (_, value) =>
            {
                TaskHelper.RunSync(_domainEventMediator.ExecuteDomainSubscribers(value, stoppingToken));
            });
        }
    }
}
