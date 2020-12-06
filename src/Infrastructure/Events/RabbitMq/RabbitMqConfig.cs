using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqConfig
    {
        private readonly ConnectionFactory _connectionFactory;

        private static IConnection ConnectionPrivate { get; set; }
        private static IModel ChannelPrivate { get; set; }

        public RabbitMqConfig(IOptions<RabbitMqConfigParams> rabbitMqParams)
        {
            var configParams = rabbitMqParams.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = configParams.HostName,
                UserName = configParams.Username,
                Password = configParams.Password,
                Port = configParams.Port
            };
        }

        public IConnection Connection()
        {
            return ConnectionPrivate ??= _connectionFactory.CreateConnection();
        }

        public IModel Channel()
        {
            return ChannelPrivate ??= Connection().CreateModel();
        }
    }
}