using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.ActiveMq;

/// <summary>
/// Redis domain event consumer background service
/// </summary>
public class ActiveMqConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public ActiveMqConsumer(IServiceScopeFactory serviceScopeFactory)
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
        var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<ActiveMqConsumer>>();

        try
        {
            var domainEventMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
            var configuration = scope.ServiceProvider.GetRequiredService<IOptions<ActiveMqConfiguration>>();

            var connecturi = new Uri($"{configuration.Value.BrokerUri}?wireFormat.maxInactivityDuration=0");
            var connectionFactory = new ConnectionFactory(connecturi);

            // Create a Connection
            using var connection =
                await connectionFactory.CreateConnectionAsync(configuration.Value.UserName,
                    configuration.Value.Password);

            await connection.StartAsync();

            // Create a Session
            using var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge);

            var consumer1 = await ConsumeTopics(session, domainEventMediator, logger);

            var consumer2 = await ConsumeQueue(configuration, session, domainEventMediator, logger);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
            }

            consumer1.Dispose();
            consumer2.Dispose();
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
        }
    }

    private async Task<IMessageConsumer> ConsumeQueue(IOptions<ActiveMqConfiguration> configuration, ISession session,
        IRequestMediator domainEventMediator, ICustomLogger<ActiveMqConsumer> logger)
    {
        var destination = new ActiveMQQueue(configuration.Value.Queue);

        var consumer = await session.CreateConsumerAsync(destination);

        consumer.Listener += message =>
        {
            try
            {
                if (message is not ITextMessage textMessage)
                    return;

                TaskHelper.RunSync(domainEventMediator.Execute(textMessage.Text, typeof(ICommandRequestHandler<>),
                    nameof(ICommandRequestHandler<CommandRequest>.Handle), CancellationToken.None));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        };

        return consumer;
    }

    private async Task<IMessageConsumer> ConsumeTopics(ISession session, IRequestMediator domainEventMediator,
        ICustomLogger<ActiveMqConsumer> logger)
    {
        const string topicPattern = ">"; // Utiliza el comodín ">" para suscribirte a todos los topics
        var destination = new ActiveMQTopic(topicPattern);

        var consumer = await session.CreateConsumerAsync(destination);

        consumer.Listener += message =>
        {
            try
            {
                if (message is not ITextMessage textMessage)
                    return;

                TaskHelper.RunSync(domainEventMediator.Execute(textMessage.Text,
                    typeof(IDomainEventSubscriber<>), nameof(IDomainEventSubscriber<DomainEvent>.On),
                    CancellationToken.None));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        };

        return consumer;
    }
}
