using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.ApacheActiveMq
{
    /// <summary> </summary>
    public class ApacheActiveMqPublisher
    {
        private readonly string _brokerUri;

        /// <summary> </summary>
        public ApacheActiveMqPublisher(string brokerUri)
        {
            _brokerUri = brokerUri;
        }

        /// <summary> </summary>
        public Task PublishTopic(string textMessage, string topicName)
        {
            return PublishCommon(textMessage, topicName: topicName);
        }

        /// <summary> </summary>
        public Task PublishOnQueue(string textMessage, string queue)
        {
            return PublishCommon(textMessage, queue: queue);
        }

        private async Task PublishCommon(string textMessage, string queue = default, string topicName = default)
        {
            var connecturi = new Uri(_brokerUri);
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
            else if (!string.IsNullOrWhiteSpace(queue))
            {
                destination = await session.GetQueueAsync(queue);
            }
            else
            {
                throw new ArgumentNullException(
                    $"At least one value must not be empty. {nameof(queue)} or {nameof(topicName)}");
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
