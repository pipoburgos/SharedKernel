using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Cqrs.Commands;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    public class InMemoryBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InMemoryBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var subscribers = scope.ServiceProvider.GetRequiredService<DomainEventsToExecute>().Subscribers.ToList();

                    foreach (var subscriber in subscribers)
                    {
                        await subscriber(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider
                        .GetRequiredService<ICustomLogger<QueuedHostedService>>()
                        .Error(ex, "Error occurred executing event.");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
