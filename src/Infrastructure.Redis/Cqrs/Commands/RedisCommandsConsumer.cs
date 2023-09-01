using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Cqrs.Commands;

/// <summary> Redis domain event consumer background service </summary>
internal class RedisCommandsConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary> Constructor </summary>
    public RedisCommandsConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>  </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(5_000, stoppingToken);
        using var scope = _serviceScopeFactory.CreateScope();
        var connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        var requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
        var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<RedisCommandsConsumer>>();
        while (!stoppingToken.IsCancellationRequested)
        {
            var value = await connectionMultiplexer.GetDatabase().ListLeftPopAsync("CommandsQueue");

            if (value.HasValue)
            {
                try
                {
                    await requestMediator.Execute(value.ToString(), typeof(ICommandRequestHandler<>),
                        nameof(ICommandRequestHandler<CommandRequest>.Handle), CancellationToken.None);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
