using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Infrastructure.Hosting;

/// <summary> . </summary>
public abstract class BackgroundServiceBase : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan? _delay;

    /// <summary> . </summary>
    /// <param name="serviceScopeFactory"></param>
    /// <param name="delay">Set to 5 minutes by default. </param>
    protected BackgroundServiceBase(IServiceScopeFactory serviceScopeFactory, TimeSpan? delay = default)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _delay = delay;
    }

    /// <summary> . </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(_delay ?? TimeSpan.FromMinutes(5), stoppingToken);

        using var scope = _serviceScopeFactory.CreateScope();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ExecuteAsync(scope, CancellationToken.None);
            }
            catch (Exception ex)
            {
                scope.ServiceProvider
                    .GetRequiredService<ILogger<BackgroundServiceBase>>()
                    .LogError(ex, "Error occurred executing background service.");
            }
        }
    }

    /// <summary> . </summary>
    /// <param name="scope"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken);
}