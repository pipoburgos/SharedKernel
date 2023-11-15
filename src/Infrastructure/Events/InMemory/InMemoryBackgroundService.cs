using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Infrastructure.Events.InMemory;

/// <summary>
/// 
/// </summary>
public class InMemoryBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public InMemoryBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Execute(scope);
        }
    }

    private static async Task Execute(IServiceScope scope)
    {
        try
        {
            await scope.ServiceProvider.GetRequiredService<IInMemoryDomainEventsConsumer>()
                .ExecuteAll(CancellationToken.None);
        }
        catch (Exception ex)
        {
            scope.ServiceProvider
                .GetRequiredService<ILogger<InMemoryBackgroundService>>()
                .LogError(ex, "Error occurred executing event.");
        }
    }
}