using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Events.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisEventBus : IEventBus
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly DomainEventJsonSerializer _domainEventJsonSerializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionMultiplexer"></param>
        /// <param name="executeMiddlewaresService"></param>
        /// <param name="domainEventJsonSerializer"></param>
        public RedisEventBus(
            IConnectionMultiplexer connectionMultiplexer,
            ExecuteMiddlewaresService executeMiddlewaresService,
            DomainEventJsonSerializer domainEventJsonSerializer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _executeMiddlewaresService = executeMiddlewaresService;
            _domainEventJsonSerializer = domainEventJsonSerializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, (req, _) =>
            {
                var eventAsString = _domainEventJsonSerializer.Serialize(req);
                return _connectionMultiplexer.GetSubscriber().PublishAsync("*", eventAsString);
            });
        }
    }
}
