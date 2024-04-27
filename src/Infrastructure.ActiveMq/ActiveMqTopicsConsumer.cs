using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.ActiveMq;

/// <summary> ActiveMq domain events consumer. </summary>
public class ActiveMqTopicsConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary> Constructor. </summary>
    public ActiveMqTopicsConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary> Execute. </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var domainEventSubscriberType = typeof(IDomainEventSubscriber<>);
        const string method = nameof(IDomainEventSubscriber<DomainEvent>.On);

        using var scope = _serviceScopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ActiveMqTopicsConsumer>>();

        var requestMediator = scope.ServiceProvider.GetRequiredService<IRequestMediator>();
        var configuration = scope.ServiceProvider.GetRequiredService<IOptions<ActiveMqConfiguration>>();

        var connecturi = new Uri($"{configuration.Value.BrokerUri}?wireFormat.maxInactivityDuration=0");
        var connectionFactory = new ConnectionFactory(connecturi);

        if (!string.IsNullOrWhiteSpace(configuration.Value.UserName) &&
            !string.IsNullOrWhiteSpace(configuration.Value.Password))
        {
            connectionFactory.UserName = configuration.Value.UserName;
            connectionFactory.Password = configuration.Value.Password;
        }

        // Create a Connection
        using var connection = await connectionFactory.CreateConnectionAsync();

        await connection.StartAsync();

        // Create a Session
        using var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge);

        const string topicPattern = ">"; // Utiliza el comodín ">" para suscribirte a todos los topics
        var destination = new ActiveMQTopic(topicPattern);

        var consumer = await session.CreateConsumerAsync(destination);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = await consumer.ReceiveAsync();

                if (message is not ITextMessage textMessage ||
                    !requestMediator.HandlerImplemented(textMessage.Text, domainEventSubscriberType))
                    continue;

                await requestMediator.Execute(textMessage.Text, domainEventSubscriberType, method,
                    CancellationToken.None);

                await message.AcknowledgeAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        await consumer.CloseAsync();
        await session.CloseAsync();
        await connection.CloseAsync();
    }
}
