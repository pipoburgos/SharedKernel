using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Commands.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class QueuedHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public QueuedHostedService(IServiceScopeFactory serviceScopeFactory)
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

            scope.ServiceProvider
                .GetRequiredService<ICustomLogger<QueuedHostedService>>()
                .Info("Queued Hosted Service is running for async commands");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await scope.ServiceProvider
                    .GetRequiredService<IBackgroundTaskQueue>()
                    .DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider
                        .GetRequiredService<ICustomLogger<QueuedHostedService>>()
                        .Error(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            scope.ServiceProvider
                .GetRequiredService<ICustomLogger<QueuedHostedService>>().Info("Queued Hosted Service is stopping.");

            while (scope.ServiceProvider.GetRequiredService<IBackgroundTaskQueue>().Any())
            {
                var workItem = await scope.ServiceProvider
                    .GetRequiredService<IBackgroundTaskQueue>()
                    .DequeueAsync(cancellationToken);

                try
                {
                    await workItem(cancellationToken);
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider
                        .GetRequiredService<ICustomLogger<QueuedHostedService>>().Error(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            scope.ServiceProvider
                .GetRequiredService<ICustomLogger<QueuedHostedService>>().Info("Queued Hosted Service is stopped.");

            await base.StopAsync(cancellationToken);
        }
    }
}
