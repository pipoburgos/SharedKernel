using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
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
        using var scope = _serviceScopeFactory.CreateScope();
        var connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        var requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<RedisCommandsConsumer>>();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (connectionMultiplexer.IsConnected)
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
                            logger.LogError(ex, ex.Message);
                        }
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
            catch (RedisConnectionException e)
            {
                logger.LogError(e, e.Message);
            }
        }
    }
}
