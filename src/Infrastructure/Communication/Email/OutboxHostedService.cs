using Microsoft.Extensions.Hosting;

namespace SharedKernel.Infrastructure.Communication.Email;
internal class OutboxHostedService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
