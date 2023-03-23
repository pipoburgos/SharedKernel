using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Hosting
{
    /// <summary>  </summary>
    public abstract class BackgroundServiceBase : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly int _minutesDelay;

        /// <summary>  </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="minutesDelay"></param>
        protected BackgroundServiceBase(IServiceScopeFactory serviceScopeFactory, int minutesDelay = 2)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _minutesDelay = minutesDelay;
        }

        /// <summary>  </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(_minutesDelay), stoppingToken);

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
                        .GetRequiredService<ICustomLogger<BackgroundServiceBase>>()
                        .Error(ex, "Error occurred executing background service.");
                }
            }
        }

        /// <summary>  </summary>
        /// <param name="scope"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken);
    }
}
