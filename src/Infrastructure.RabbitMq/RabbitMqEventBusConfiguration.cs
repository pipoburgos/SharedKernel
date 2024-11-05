using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
public class RabbitMqEventBusConfiguration : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary> . </summary>
    public RabbitMqEventBusConfiguration(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary> . </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var channel = scope.ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Channel();
        var requestsTypes = scope.ServiceProvider.GetServices<IRequestType>().ToList();

        ConsumeQueue(scope, channel, requestsTypes.Where(rt => !rt.IsTopic));
        ConsumeTopics(scope, channel, requestsTypes.Where(rt => rt.IsTopic));

        return Task.CompletedTask;
    }

    private static void ConsumeQueue(IServiceScope scope, IModel channel, IEnumerable<IRequestType> requestsTypes)
    {
        var exchangeName = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqConfigParams>>().Value.ConsumeQueue;

        var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(exchangeName);
        var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(exchangeName);

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.ExchangeDeclare(retryDomainEventExchange, ExchangeType.Direct);
        channel.ExchangeDeclare(deadLetterDomainEventExchange, ExchangeType.Direct);

        var consumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();
        foreach (var requestType in requestsTypes)
        {
            var requestQueueName = RabbitMqQueueNameFormatter.Format(requestType);
            var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(requestType);
            var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(requestType);

            var queue = channel.QueueDeclare(requestQueueName, true, false, false);

            var retryQueue = channel.QueueDeclare(retryQueueName, true, false, false,
                RetryQueueArguments(exchangeName, requestQueueName));

            var deadLetterQueue = channel.QueueDeclare(deadLetterQueueName, true, false, false);

            channel.QueueBind(queue, exchangeName, requestQueueName);
            channel.QueueBind(retryQueue, retryDomainEventExchange, requestQueueName);
            channel.QueueBind(deadLetterQueue, deadLetterDomainEventExchange, requestQueueName);
            channel.QueueBind(queue, exchangeName, requestType.UniqueName);
            consumer.ConsumeQueue(requestType.UniqueName);
        }
    }

    private static void ConsumeTopics(IServiceScope scope, IModel channel, IEnumerable<IRequestType> requestsTypes)
    {
        var exchangeName = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqConfigParams>>().Value.ExchangeName;

        var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(exchangeName);
        var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(exchangeName);

        channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        channel.ExchangeDeclare(retryDomainEventExchange, ExchangeType.Topic);
        channel.ExchangeDeclare(deadLetterDomainEventExchange, ExchangeType.Topic);

        var consumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();
        foreach (var requestType in requestsTypes)
        {
            var domainEventsQueueName = RabbitMqQueueNameFormatter.Format(requestType);
            var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(requestType);
            var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(requestType);

            var queue = channel.QueueDeclare(domainEventsQueueName, true, false, false);

            var retryQueue = channel.QueueDeclare(retryQueueName, true, false, false,
                RetryQueueArguments(exchangeName, domainEventsQueueName));

            var deadLetterQueue = channel.QueueDeclare(deadLetterQueueName, true, false, false);

            channel.QueueBind(queue, exchangeName, domainEventsQueueName);
            channel.QueueBind(retryQueue, retryDomainEventExchange, domainEventsQueueName);
            channel.QueueBind(deadLetterQueue, deadLetterDomainEventExchange, domainEventsQueueName);
            channel.QueueBind(queue, exchangeName, requestType.UniqueName);
            consumer.ConsumeTopic(requestType.UniqueName);
        }
    }

    private static IDictionary<string, object> RetryQueueArguments(string domainEventExchange,
        string domainEventQueue)
    {
        return new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", domainEventExchange},
                {"x-dead-letter-routing-key", domainEventQueue},
                {"x-message-ttl", 1000},
            };
    }
}
