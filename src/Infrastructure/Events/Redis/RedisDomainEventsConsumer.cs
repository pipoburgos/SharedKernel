using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
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
        private readonly IRequestMediator _requestMediator;
        private readonly ICustomLogger<RedisDomainEventsConsumer> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public RedisDomainEventsConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();

            _connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            _requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
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
                    TaskHelper.RunSync(_requestMediator.Execute(value, typeof(IDomainEventSubscriber<>),
                        nameof(IDomainEventSubscriber<DomainEvent>.On), CancellationToken.None));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                }
            });
        }
    }
}
