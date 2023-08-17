using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Commands.Redis;

/// <summary> Redis domain event consumer background service </summary>
public class RedisCommandsConsumer : BackgroundService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IRequestMediator _requestMediator;
    private readonly ICustomLogger<RedisCommandsConsumer> _logger;

    /// <summary> Constructor </summary>
    /// <param name="serviceScopeFactory"></param>
    public RedisCommandsConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        using var scope = serviceScopeFactory.CreateScope();

        _connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        _requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
        _logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<RedisCommandsConsumer>>();
    }

    /// <summary>  </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var value = await _connectionMultiplexer.GetDatabase().ListLeftPopAsync("CommandsQueue");

            try
            {
                TaskHelper.RunSync(_requestMediator.Execute(value, typeof(ICommandRequestHandler<>),
                    nameof(ICommandRequestHandler<CommandRequest>.Handle), CancellationToken.None));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}

