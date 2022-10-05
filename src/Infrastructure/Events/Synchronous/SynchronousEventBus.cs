using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Synchronous
{
    /// <summary>
    /// 
    /// </summary>
    public class SynchronousEventBus : IEventBus
    {
        private readonly IDomainEventJsonSerializer _serializer;
        private readonly IDomainEventMediator _domainEventMediator;

        /// <summary> Contructor. </summary>
        /// <param name="serializer"></param>
        /// <param name="domainEventMediator"></param>
        public SynchronousEventBus(
            IDomainEventJsonSerializer serializer,
            IDomainEventMediator domainEventMediator)
        {
            _serializer = serializer;
            _domainEventMediator = domainEventMediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            var eventSerialized = _serializer.Serialize(@event);
            return _domainEventMediator.ExecuteDomainSubscribers(eventSerialized, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken"></param>
        public async Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            foreach (var @event in events)
            {
                var eventSerialized = _serializer.Serialize(@event);
                await _domainEventMediator.ExecuteDomainSubscribers(eventSerialized, cancellationToken);
            }
        }
    }
}
