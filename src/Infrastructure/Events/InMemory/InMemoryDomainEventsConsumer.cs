using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary> </summary>
    public class InMemoryDomainEventsConsumer : IInMemoryDomainEventsConsumer
    {
        private readonly EventQueue _eventQueue;
        private readonly IDomainEventMediator _domainEventMediator;
        private readonly IDomainEventJsonSerializer _serializer;

        /// <summary>  </summary>
        public InMemoryDomainEventsConsumer(
            EventQueue eventQueue,
            IDomainEventMediator domainEventMediator,
            IDomainEventJsonSerializer serializer)
        {
            _eventQueue = eventQueue;
            _domainEventMediator = domainEventMediator;
            _serializer = serializer;

        }

        /// <summary>  </summary>
        /// <param name="domainEvent"></param>
        public void Add(DomainEvent domainEvent)
        {
            var eventSerialized = _serializer.Serialize(domainEvent);
            _eventQueue.Enqueue(eventSerialized);
        }

        /// <summary>  </summary>
        /// <param name="domainEvents"></param>
        public void AddRange(IEnumerable<DomainEvent> domainEvents)
        {
            foreach (var @event in domainEvents)
            {
                var eventSerialized = _serializer.Serialize(@event);
                _eventQueue.Enqueue(eventSerialized);
            }
        }

        /// <summary>  </summary>
        public async Task ExecuteAll(CancellationToken cancellationToken)
        {
            while (_eventQueue.TryDequeue(out var domainEvent))
            {
                await _domainEventMediator.ExecuteDomainSubscribers(domainEvent, cancellationToken);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        }
    }
}
