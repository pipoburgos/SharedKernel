using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMqConnectionFactory
    {
        private readonly ConnectionFactory _connectionFactory;

        private static IConnection ConnectionPrivate { get; set; }
        private static IModel ChannelPrivate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rabbitMqParams"></param>
        public RabbitMqConnectionFactory(IOptions<RabbitMqConfigParams> rabbitMqParams)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IConnection Connection()
        {
            return ConnectionPrivate ??= _connectionFactory.CreateConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IModel Channel()
        {
            return ChannelPrivate ??= Connection().CreateModel();
        }
    }
}