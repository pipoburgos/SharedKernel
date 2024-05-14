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

        // Create a Connection
        using var connection = await connectionFactory.CreateConnectionAsync();

        await connection.StartAsync();

        using var session = await connection.CreateSessionAsync(AcknowledgementMode.ClientAcknowledge);

        var destination = new ActiveMQQueue(configuration.Value.ConsumeQueue);

        var consumer = await session.CreateConsumerAsync(destination);

        var tasks = new List<Task>();
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await consumer.ReceiveAsync();

            var executing = Task.Run(() =>
                Send(message, requestMediator, commandRequestHandlerType, method, consumer, logger), stoppingToken);

            tasks.Add(executing);
        }

        await Task.WhenAll(tasks);
        await session.CloseAsync();
        await connection.CloseAsync();
    }

    private async Task Send(IMessage message, IRequestMediator requestMediator, Type commandRequestHandlerType,
        string method, IMessageConsumer consumer, ILogger<ActiveMqTopicsConsumer> logger)
    {
        try
        {
            if (message is not ITextMessage textMessage ||
            !requestMediator.HandlerImplemented(textMessage.Text, commandRequestHandlerType))
                return;

            await requestMediator.Execute(textMessage.Text, commandRequestHandlerType, method,
                CancellationToken.None);

            await message.AcknowledgeAsync();
            await consumer.CloseAsync();
            consumer.Dispose();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }
}
