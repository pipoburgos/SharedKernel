using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.ApacheActiveMq
{
    /// <summary> </summary>
    public class ApacheActiveMqPublisher
    {
        private readonly ApacheActiveMqConfiguration _configuration;

        /// <summary> </summary>
        public ApacheActiveMqPublisher(IOptions<ApacheActiveMqConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        /// <summary> </summary>
        public Task PublishTopic(string textMessage, string topicName)
        {
            return PublishCommon(textMessage, topicName: topicName);
        }

        /// <summary> </summary>
        public Task PublishOnQueue(string textMessage)
        {
            return PublishCommon(textMessage);
        }

        private async Task PublishCommon(string textMessage, string topicName = default)
        {
            var connecturi = new Uri(_configuration.BrokerUri);
            var connectionFactory = new ConnectionFactory(connecturi);

            // Create a Connection
            using var connection = await connectionFactory.CreateConnectionAsync();

            await connection.StartAsync();

            // Create a Session
            using var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge);

            IDestination destination;
            if (!string.IsNullOrWhiteSpace(topicName))
            {
                destination = await session.GetTopicAsync(topicName);
            }
            else if (!string.IsNullOrWhiteSpace(_configuration.Queue))
            {
                destination = await session.GetQueueAsync(_configuration.Queue);
            }
            else
            {
                throw new ArgumentNullException(
                    $"At least one value must not be empty. {nameof(_configuration.Queue)} or {nameof(topicName)}");
            }

            // Create a MessageProducer from the Session to the Topic or Queue
            var producer = await session.CreateProducerAsync(destination);
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            var message = await session.CreateTextMessageAsync(textMessage);

            // Tell the producer to send the message
            await producer.SendAsync(message);

            await connection.CloseAsync();
        }
    }
}
