using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisEventBus : IEventBus
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDomainEventJsonSerializer _domainEventJsonSerializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionMultiplexer"></param>
        /// <param name="domainEventJsonSerializer"></param>
        public RedisEventBus(
            IConnectionMultiplexer connectionMultiplexer,
            IDomainEventJsonSerializer domainEventJsonSerializer)
        {
            _connectionMultiplexer = connectionMultiplexer;
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
            var eventAsString = _domainEventJsonSerializer.Serialize(@event);
            return _connectionMultiplexer.GetSubscriber().PublishAsync(RedisChannel.Pattern("*"), eventAsString);
        }
    }
}
