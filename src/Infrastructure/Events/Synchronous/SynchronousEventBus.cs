using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
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
        private readonly IRequestSerializer _requestSerializer;
        private readonly IRequestMediator _requestMediator;

        /// <summary> Contructor. </summary>
        /// <param name="requestSerializer"></param>
        /// <param name="requestMediator"></param>
        public SynchronousEventBus(
            IRequestSerializer requestSerializer,
            IRequestMediator requestMediator)
        {
            _requestSerializer = requestSerializer;
            _requestMediator = requestMediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            var eventSerialized = _requestSerializer.Serialize(@event);
            return _requestMediator.Execute(eventSerialized, typeof(IDomainEventSubscriber<>), nameof(IDomainEventSubscriber<DomainEvent>.On), cancellationToken);
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
                var eventSerialized = _requestSerializer.Serialize(@event);
                await _requestMediator.Execute(eventSerialized, typeof(IDomainEventSubscriber<>), nameof(IDomainEventSubscriber<DomainEvent>.On),
                    cancellationToken);
            }
        }
    }
}
