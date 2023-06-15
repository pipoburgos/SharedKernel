using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.ApacheActiveMq
{
    /// <summary>
    /// Redis domain event consumer background service
    /// </summary>
    public class ApacheActiveMqDomainEventsConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public ApacheActiveMqDomainEventsConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger<ApacheActiveMqDomainEventsConsumer>>();

            try
            {
                var domainEventMediator = scope.ServiceProvider.GetRequiredService<IDomainEventMediator>();
                var configuration = scope.ServiceProvider.GetRequiredService<IOptions<ApacheActiveMqConfiguration>>();

                var connecturi = new Uri($"{configuration.Value.BrokerUri}?wireFormat.maxInactivityDuration=0");
                var connectionFactory = new ConnectionFactory(connecturi);

                // Create a Connection
                using var connection = await connectionFactory.CreateConnectionAsync();

                await connection.StartAsync();

                // Create a Session
                using var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge);


                const string topicPattern = ">"; // Utiliza el comodín ">" para suscribirte a todos los topics
                var destination = new ActiveMQTopic(topicPattern);

                using var consumer = await session.CreateConsumerAsync(destination);

                while (!stoppingToken.IsCancellationRequested)
                {
                    consumer.Listener += message =>
                    {
                        try
                        {
                            if (message is not ITextMessage textMessage)
                                return;

                            TaskHelper.RunSync(domainEventMediator.ExecuteDomainSubscribers(textMessage!.Text, CancellationToken.None));
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, ex.Message);
                        }
                    };

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }
    }
}
