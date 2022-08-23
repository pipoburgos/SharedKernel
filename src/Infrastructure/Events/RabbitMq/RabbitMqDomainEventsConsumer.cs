using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers;
using SharedKernel.Infrastructure.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    internal class RabbitMqDomainEventsConsumer
    {
        private readonly RabbitMqConnectionFactory _config;
        private readonly IDomainEventMediator _domainEventMediator;
        private readonly ICustomLogger<RabbitMqDomainEventsConsumer> _logger;
        private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;
        private readonly IDomainEventSubscriberProviderFactory _domainEventSubscriberProviderFactory;
        private readonly IDomainEventJsonDeserializer _deserializer;
        private readonly RetrieverOptions _retrieverOptions;
        private const string HeaderRedelivery = "redelivery_count";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="config"></param>
        /// <param name="domainEventMediator"></param>
        /// <param name="logger"></param>
        /// <param name="rabbitMqParams"></param>
        /// <param name="options"></param>
        /// <param name="domainEventSubscriberProviderFactory"></param>
        public RabbitMqDomainEventsConsumer(
            IDomainEventJsonDeserializer deserializer,
            RabbitMqConnectionFactory config,
            IDomainEventMediator domainEventMediator,
            ICustomLogger<RabbitMqDomainEventsConsumer> logger,
            IOptions<RabbitMqConfigParams> rabbitMqParams,
            IOptionsService<RetrieverOptions> options,
            IDomainEventSubscriberProviderFactory domainEventSubscriberProviderFactory)
        {
            _deserializer = deserializer;
            _config = config;
            _domainEventMediator = domainEventMediator;
            _logger = logger;
            _rabbitMqParams = rabbitMqParams;
            _domainEventSubscriberProviderFactory = domainEventSubscriberProviderFactory;
            _retrieverOptions = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Consume()
        {
            _domainEventSubscriberProviderFactory.GetAll().ForEach(a => ConsumeMessages(a.SubscriberName(), a.GetSubscriber()));
            return Task.CompletedTask;
        }

        private void ConsumeMessages(string queue, Type eventSubscriberType, ushort prefetchCount = 10)
        {
            var channel = _config.Channel();

            DeclareQueue(channel, queue);

            channel.BasicQos(0, prefetchCount, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var @event = _deserializer.Deserialize(message);

                    TaskHelper.RunSync(_domainEventMediator.ExecuteOn(message, @event, eventSubscriberType, CancellationToken.None));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                    HandleConsumptionError(ea, queue);
                }

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue, false, consumer);
        }

        private static void DeclareQueue(IModel channel, string queue)
        {
            channel.QueueDeclare(queue, true, false, false);
        }

        private void HandleConsumptionError(BasicDeliverEventArgs ea, string queue)
        {
            if (HasBeenRedeliveredTooMuch(ea.BasicProperties.Headers))
                SendToDeadLetter(ea, queue);
            else
                SendToRetry(ea, queue);
        }

        private bool HasBeenRedeliveredTooMuch(IDictionary<string, object> headers)
        {
            return (int)(headers[HeaderRedelivery] ?? 0) >= _retrieverOptions.RetryCount;
        }

        private void SendToRetry(BasicDeliverEventArgs ea, string queue)
        {
            SendMessageTo(RabbitMqExchangeNameFormatter.Retry(_rabbitMqParams.Value.ExchangeName), ea, queue);
        }

        private void SendToDeadLetter(BasicDeliverEventArgs ea, string queue)
        {
            SendMessageTo(RabbitMqExchangeNameFormatter.DeadLetter(_rabbitMqParams.Value.ExchangeName), ea, queue);
        }

        private void SendMessageTo(string exchange, BasicDeliverEventArgs ea, string queue)
        {
            var channel = _config.Channel();
            channel.ExchangeDeclare(exchange, ExchangeType.Topic);

            var body = ea.Body;
            var properties = ea.BasicProperties;
            var headers = ea.BasicProperties.Headers;
            headers[HeaderRedelivery] = (int)headers[HeaderRedelivery] + 1;
            properties.Headers = headers;

            channel.BasicPublish(exchange, queue, properties, body);
        }
    }
}