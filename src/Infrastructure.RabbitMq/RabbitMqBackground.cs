using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
public class RabbitMqBackground : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IServiceScope _serviceScope = null!;
    private RabbitMqConsumer _rabbitMqConsumer = null!;

    /// <summary> . </summary>
    public RabbitMqBackground(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _serviceScope = _serviceScopeFactory.CreateScope();
        _rabbitMqConsumer = _serviceScope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();
        await _rabbitMqConsumer.StartAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _rabbitMqConsumer.DisposeAsync();
        _serviceScope.Dispose();
    }

}
