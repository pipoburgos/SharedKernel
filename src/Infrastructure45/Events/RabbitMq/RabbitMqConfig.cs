using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqConfig
    {
        public ConnectionFactory ConnectionFactory { get; private set; }
        private static IConnection ConnectionPrivate { get; set; }
        private static IModel ChannelPrivate { get; set; }

        public RabbitMqConfig(IOptions<RabbitMqConfigParams> rabbitMqParams)
        {
            var configParams = rabbitMqParams.Value;

            ConnectionFactory = new ConnectionFactory
            {
                HostName = configParams.HostName,
                UserName = configParams.Username,
                Password = configParams.Password,
                Port = configParams.Port
            };
        }

        public IConnection Connection()
        {
            return ConnectionPrivate ?? (ConnectionPrivate = ConnectionFactory.CreateConnection());
        }

        public IModel Channel()
        {
            return ChannelPrivate ?? (ChannelPrivate = Connection().CreateModel());
        }
    }
}