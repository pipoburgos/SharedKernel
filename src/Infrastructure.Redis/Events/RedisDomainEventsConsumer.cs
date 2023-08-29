using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Events;

/// <summary> Redis domain event consumer background service. </summary>
internal class RedisCommandsConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary> Constructor. </summary>
    /// <param name="serviceScopeFactory"></param>
    public RedisCommandsConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary> </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        var requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
        var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<RedisCommandsConsumer>>();

        return connectionMultiplexer.GetSubscriber().SubscribeAsync(RedisChannel.Pattern("*"), (_, value) =>
        {
            try
            {
                TaskHelper.RunSync(requestMediator.Execute(value.ToString(), typeof(IDomainEventSubscriber<>),
                    nameof(IDomainEventSubscriber<DomainEvent>.On), CancellationToken.None));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        });
    }
}
