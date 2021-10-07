using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
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
                try
                {
                    await scope.ServiceProvider.GetRequiredService<IInMemoryDomainEventsConsumer>().ExecuteAll(stoppingToken);
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider
                        .GetRequiredService<ICustomLogger<InMemoryBackgroundService>>()
                        .Error(ex, "Error occurred executing event.");
                }
            }
        }
    }
}
