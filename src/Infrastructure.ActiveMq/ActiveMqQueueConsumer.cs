using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.ActiveMq;

/// <summary> ActiveMq commands consumer. </summary>
public class ActiveMqQueueConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary> Constructor. </summary>
    public ActiveMqQueueConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary> Execute. </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandRequestHandlerType = typeof(ICommandRequestHandler<>);
        const string method = nameof(ICommandRequestHandler<CommandRequest>.Handle);

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

        using var connection = await connectionFactory.CreateConnectionAsync();
        await connection.StartAsync();
        using var session = await connection.CreateSessionAsync(AcknowledgementMode.ClientAcknowledge);
        using var destination = new ActiveMQQueue(configuration.Value.ConsumeQueue);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumer = await session.CreateConsumerAsync(destination);
                logger.LogWarning("Escuchando...");
                var message = await consumer.ReceiveAsync();
                logger.LogWarning($"Mensaje {message.NMSCorrelationID} recibido.");
                await Send(consumer, message, requestMediator, commandRequestHandlerType, method, logger);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
        finally
        {
            await session.CloseAsync();
            await connection.CloseAsync();
        }
    }

    private static async Task Send(IMessageConsumer consumer, IMessage message, IRequestMediator requestMediator,
        Type commandRequestHandlerType, string method, ILogger<ActiveMqTopicsConsumer> logger)
    {
        try
        {
            if (message is not ITextMessage textMessage ||
                !requestMediator.HandlerImplemented(textMessage.Text, commandRequestHandlerType))
                return;

            await requestMediator.Execute(textMessage.Text, commandRequestHandlerType, method, CancellationToken.None);

            await message.AcknowledgeAsync();

            logger.LogWarning("Mensaje procesado.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
        finally
        {
            await consumer.CloseAsync();
            consumer.Dispose();
        }
    }
}