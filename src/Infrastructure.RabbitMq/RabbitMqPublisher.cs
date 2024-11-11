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
    private readonly RabbitMqConnectionFactory _config;
    private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;

    /// <summary> . </summary>
    public RabbitMqPublisher(
        ILogger<RabbitMqPublisher> logger,
        RabbitMqConnectionFactory config,
        IOptions<RabbitMqConfigParams> rabbitMqParams)
    {
        _logger = logger;
        _config = config;
        _rabbitMqParams = rabbitMqParams;
    }

    /// <summary> </summary>
    public Task PublishTopic(string textMessage, string topicName, CancellationToken cancellationToken)
    {
        return PublishCommon(textMessage, true, topicName, cancellationToken);
    }

    /// <summary> </summary>
    public Task PublishOnQueue(string textMessage, string queue, CancellationToken cancellationToken)
    {
        return PublishCommon(textMessage, false, queue, cancellationToken);
    }

    private async Task PublishCommon(string textMessage, bool isTopic, string name, CancellationToken cancellationToken)
    {
        try
        {
            var queue = isTopic ? _rabbitMqParams.Value.ExchangeName : _rabbitMqParams.Value.PublishQueue;
            var exchangeType = isTopic ? ExchangeType.Topic : ExchangeType.Direct;

            var channel = await _config.CreateChannelAsync(cancellationToken);
            await channel.ExchangeDeclareAsync(queue, exchangeType, cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(textMessage);
            var properties = new BasicProperties
            {
                Headers = new Dictionary<string, object?> { { HeaderReDelivery, 0 } },
            };

            await channel.BasicPublishAsync(queue, name, true, properties, body, cancellationToken: cancellationToken);
        }
        catch (RabbitMQClientException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
