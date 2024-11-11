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
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var ct = CancellationToken.None;

        using var scope = _serviceScopeFactory.CreateScope();

        var channel = await scope.ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().CreateChannelAsync(ct);
        var requestsTypes = scope.ServiceProvider.GetServices<IRequestType>().ToList();

        await ConsumeQueue(scope, channel, requestsTypes.Where(rt => !rt.IsTopic), ct);
        await ConsumeTopics(scope, channel, requestsTypes.Where(rt => rt.IsTopic), ct);
    }

    private static async Task ConsumeQueue(IServiceScope scope, IChannel channel,
        IEnumerable<IRequestType> requestsTypes, CancellationToken cancellationToken)
    {
        var exchangeName = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqConfigParams>>().Value.ConsumeQueue;

        var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(exchangeName);
        var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(exchangeName);

        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(retryDomainEventExchange, ExchangeType.Direct, cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(deadLetterDomainEventExchange, ExchangeType.Direct, cancellationToken: cancellationToken);

        var consumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();
        foreach (var requestType in requestsTypes)
        {
            var requestQueueName = RabbitMqQueueNameFormatter.Format(requestType);
            var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(requestType);
            var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(requestType);

            var queue = await channel.QueueDeclareAsync(requestQueueName, true, false, false,
                cancellationToken: cancellationToken);

            var retryQueue = await channel.QueueDeclareAsync(retryQueueName, true, false, false,
                RetryQueueArguments(exchangeName, requestQueueName), cancellationToken: cancellationToken);

            var deadLetterQueue = await channel.QueueDeclareAsync(deadLetterQueueName, true, false, false,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(queue, exchangeName, requestQueueName, cancellationToken: cancellationToken);

            await channel.QueueBindAsync(retryQueue, retryDomainEventExchange, requestQueueName,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(deadLetterQueue, deadLetterDomainEventExchange, requestQueueName,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(queue, exchangeName, requestType.UniqueName,
                cancellationToken: cancellationToken);

            await consumer.ConsumeQueueAsync(requestType.UniqueName, cancellationToken: cancellationToken);
        }
    }

    private static async Task ConsumeTopics(IServiceScope scope, IChannel channel,
        IEnumerable<IRequestType> requestsTypes, CancellationToken cancellationToken)
    {
        var exchangeName = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqConfigParams>>().Value.ExchangeName;

        var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(exchangeName);
        var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(exchangeName);

        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(retryDomainEventExchange, ExchangeType.Topic, cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(deadLetterDomainEventExchange, ExchangeType.Topic, cancellationToken: cancellationToken);

        var consumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();
        foreach (var requestType in requestsTypes)
        {
            var domainEventsQueueName = RabbitMqQueueNameFormatter.Format(requestType);
            var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(requestType);
            var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(requestType);

            var queue = await channel.QueueDeclareAsync(domainEventsQueueName, true, false, false, cancellationToken: cancellationToken);

            var retryQueue = await channel.QueueDeclareAsync(retryQueueName, true, false, false,
                RetryQueueArguments(exchangeName, domainEventsQueueName), cancellationToken: cancellationToken);

            var deadLetterQueue = await channel.QueueDeclareAsync(deadLetterQueueName, true, false, false, cancellationToken: cancellationToken);

            await channel.QueueBindAsync(queue, exchangeName, domainEventsQueueName, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(retryQueue, retryDomainEventExchange, domainEventsQueueName, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(deadLetterQueue, deadLetterDomainEventExchange, domainEventsQueueName, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(queue, exchangeName, requestType.UniqueName, cancellationToken: cancellationToken);
            await consumer.ConsumeTopicAsync(requestType.UniqueName, cancellationToken: cancellationToken);
        }
    }

    private static IDictionary<string, object?> RetryQueueArguments(string domainEventExchange,
        string domainEventQueue)
    {
        return new Dictionary<string, object?>
            {
                {"x-dead-letter-exchange", domainEventExchange},
                {"x-dead-letter-routing-key", domainEventQueue},
                {"x-message-ttl", 1000},
            };
    }
}
