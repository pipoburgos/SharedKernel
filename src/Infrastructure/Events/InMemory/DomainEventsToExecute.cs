using SharedKernel.Application.Logging;
using SharedKernel.Application.RetryPolicies;
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
    public class DomainEventsToExecute
    {
        private readonly DomainEventMediator _domainEventMediator;
        private readonly DomainEventJsonSerializer _serializer;
        private readonly DomainEventJsonDeserializer _deserializer;
        private readonly ICustomLogger<DomainEventsToExecute> _logger;
        private readonly IRetriever _retriever;
        private readonly ConcurrentBag<DomainEvent> _events;

        /// <summary>
        /// 
        /// </summary>
        public DomainEventsToExecute(
            DomainEventMediator domainEventMediator,
            DomainEventJsonSerializer serializer,
            DomainEventJsonDeserializer deserializer,
            ICustomLogger<DomainEventsToExecute> logger,
            IRetriever retriever)
        {
            _domainEventMediator = domainEventMediator;
            _serializer = serializer;
            _deserializer = deserializer;
            _logger = logger;
            _retriever = retriever;
            _events = new ConcurrentBag<DomainEvent>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        public void Add(DomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task ExecuteAll(CancellationToken cancellationToken)
        {
            while (_events.TryTake(out var domainEvent))
            {
                await ExecuteDomainSubscribers(domainEvent, cancellationToken);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        }

        private async Task ExecuteDomainSubscribers(DomainEvent @event, CancellationToken cancellationToken)
        {
            var subscribers = DomainEventSubscriberInformationService.GetAllEventsSubscribers(@event);
            var eventSerialized = _serializer.Serialize(@event);
            var eventDeserialized = _deserializer.Deserialize(eventSerialized);
            foreach (var subscriber in subscribers)
            {
                try
                {
                    await ExecuteDomainSubscriber(eventDeserialized, subscriber, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger?.Error(e, e.Message);
                }
            }
        }

        private Task ExecuteDomainSubscriber(DomainEvent domainEvent, string subscriber, CancellationToken cancellationToken)
        {
            return _retriever.ExecuteAsync<Task>(async ct => await _domainEventMediator.ExecuteOn(domainEvent, subscriber, ct),
                _ => true, cancellationToken);
        }
    }
}
