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
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IRequestMediator _requestMediator;
    private readonly ICustomLogger<RedisCommandsConsumer> _logger;

    /// <summary> Constructor. </summary>
    /// <param name="serviceScopeFactory"></param>
    public RedisCommandsConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        using var scope = serviceScopeFactory.CreateScope();

        _connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        _requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
        _logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<RedisCommandsConsumer>>();
    }

    /// <summary> </summary>
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
