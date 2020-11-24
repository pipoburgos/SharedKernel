using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Cqrs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ICustomLogger<QueuedHostedService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskQueue"></param>
        /// <param name="logger"></param>
        public QueuedHostedService(IBackgroundTaskQueue taskQueue,
            ICustomLogger<QueuedHostedService> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Queued Hosted Service is running for async commands");

            await BackgroundProcessing(stoppingToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Queued Hosted Service is stopping.");

            while (_taskQueue.Any())
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            _logger.Info("Queued Hosted Service is stopped.");

            await base.StopAsync(stoppingToken);
        }
    }
}
