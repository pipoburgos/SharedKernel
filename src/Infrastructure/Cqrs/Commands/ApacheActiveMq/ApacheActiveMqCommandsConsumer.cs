using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.Events.ApacheActiveMq;
using SharedKernel.Infrastructure.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Commands.ApacheActiveMq;

/// <summary>
/// Redis domain event consumer background service
/// </summary>
public class ApacheActiveMqCommandsConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public ApacheActiveMqCommandsConsumer(IServiceScopeFactory serviceScopeFactory)
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
        var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<ApacheActiveMqCommandsConsumer>>();

        try
        {
            var domainEventMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
            var configuration = scope.ServiceProvider.GetRequiredService<IOptions<ApacheActiveMqConfiguration>>();

            var connecturi = new Uri($"{configuration.Value.BrokerUri}?wireFormat.maxInactivityDuration=0");
            var connectionFactory = new ConnectionFactory(connecturi);

            // Create a Connection
            using var connection = await connectionFactory.CreateConnectionAsync();

            await connection.StartAsync();

            // Create a Session
            using var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge);


            var destination = new ActiveMQQueue(configuration.Value.Queue);

            using var consumer = await session.CreateConsumerAsync(destination);

            consumer.Listener += message =>
            {
                try
                {
                    if (message is not ITextMessage textMessage)
                        return;

                    TaskHelper.RunSync(domainEventMediator.ExecuteHandler(textMessage!.Text, CancellationToken.None));
                }
                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message);
                }
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
        }
    }
}
