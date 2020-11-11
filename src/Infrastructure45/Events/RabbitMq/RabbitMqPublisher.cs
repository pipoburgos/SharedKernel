using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqConfig _config;
        private const string HeaderReDelivery = "redelivery_count";

        public RabbitMqPublisher(RabbitMqConfig config)
        {
            _config = config;
        }

        public void Publish(string exchangeName, string eventName, string message)
        {
            var channel = _config.Channel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            var body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object> {{HeaderReDelivery, 0}};

            channel.BasicPublish(exchangeName, eventName, properties, body);
        }
    }
}