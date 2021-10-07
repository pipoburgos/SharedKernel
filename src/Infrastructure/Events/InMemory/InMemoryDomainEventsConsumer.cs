using SharedKernel.Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class InMemoryDomainEventsConsumer : IInMemoryDomainEventsConsumer
    {
        private readonly IDomainEventMediator _domainEventMediator;
        private readonly IDomainEventJsonSerializer _serializer;
        private readonly ConcurrentBag<string> _events;

        /// <summary>
        /// 
        /// </summary>
        public InMemoryDomainEventsConsumer(
            IDomainEventMediator domainEventMediator,
            IDomainEventJsonSerializer serializer)
        {
            _domainEventMediator = domainEventMediator;
            _serializer = serializer;
            _events = new ConcurrentBag<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        public void Add(DomainEvent domainEvent)
        {
            var eventSerialized = _serializer.Serialize(domainEvent);
            _events.Add(eventSerialized);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task ExecuteAll(CancellationToken cancellationToken)
        {
            while (_events.TryTake(out var domainEvent))
            {
                await _domainEventMediator.ExecuteDomainSubscribers(domainEvent, cancellationToken);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        }
    }
}
