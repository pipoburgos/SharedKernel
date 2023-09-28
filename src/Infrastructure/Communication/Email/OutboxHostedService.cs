using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Communication.Email;

namespace SharedKernel.Infrastructure.Communication.Email;
internal class OutboxHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public OutboxHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(10_000, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var senders = scope.ServiceProvider.GetServices<IEmailSender>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OutboxHostedService>>();

            var emailSender = senders.LastOrDefault(x => x.Sender);

            if (emailSender == default)
                throw new InvalidOperationException("IEmailSender EmailSender.Sender == true not registered.");

            try
            {
                var outboxMailRepository = scope.ServiceProvider.GetService<IOutboxMailRepository>();

                if (outboxMailRepository == default)
                    throw new InvalidOperationException("IOutboxMailRepository not registered.");

                var mails = await outboxMailRepository.GetPendingMails(stoppingToken);

                foreach (var outboxMail in mails)
                {
                    await emailSender.SendEmailAsync(outboxMail, stoppingToken);

                    outboxMail.Pending = true;
                    await outboxMailRepository.Update(outboxMail, stoppingToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            await Task.Delay(5_000, stoppingToken);
        }
    }
}
