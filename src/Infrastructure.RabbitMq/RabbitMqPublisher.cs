using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
public class RabbitMqPublisher
{
    private const string HeaderReDelivery = "redelivery_count";
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly RabbitMqConnectionFactory _rabbitMqConnectionFactory;
    private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;

    /// <summary> . </summary>
    public RabbitMqPublisher(
        ILogger<RabbitMqPublisher> logger,
        RabbitMqConnectionFactory rabbitMqConnectionFactory,
        IOptions<RabbitMqConfigParams> rabbitMqParams)
    {
        _logger = logger;
        _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
        _rabbitMqParams = rabbitMqParams;
    }

    /// <summary> </summary>
    public Task PublishTopic(string textMessage, string topicName, CancellationToken cancellationToken = default)
    {
        return PublishCommon(textMessage, true, topicName, cancellationToken);
    }

    /// <summary> </summary>
    public Task PublishOnQueue(string textMessage, string queue, CancellationToken cancellationToken = default)
    {
        return PublishCommon(textMessage, false, queue, cancellationToken);
    }

    private async Task PublishCommon(string textMessage, bool isTopic, string name, CancellationToken cancellationToken = default)
    {
        try
        {
            var queue = isTopic ? _rabbitMqParams.Value.ExchangeName : _rabbitMqParams.Value.PublishQueue;
            var exchangeType = isTopic ? ExchangeType.Topic : ExchangeType.Direct;

            await using var connection = await _rabbitMqConnectionFactory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(queue, exchangeType, cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(textMessage);
            var properties = new BasicProperties
            {
                Headers = new Dictionary<string, object?> { { HeaderReDelivery, 0 } },
            };

            await channel.BasicPublishAsync(queue, name, false, properties, body, cancellationToken);
        }
        catch (RabbitMQClientException ex)
        {
            _logger.LogError(ex, ex.Message);
        }

    }
}
