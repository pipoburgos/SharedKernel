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
        private readonly ConcurrentBag<string> _events;

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
                await ExecuteDomainSubscribers(domainEvent, cancellationToken);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        }

        private async Task ExecuteDomainSubscribers(string eventSerialized, CancellationToken cancellationToken)
        {
            var eventDeserialized = _deserializer.Deserialize(eventSerialized);
            var subscribers = DomainEventSubscriberInformationService.GetAllEventsSubscribers(eventDeserialized);

            foreach (var subscriber in subscribers)
            {
                try
                {
                    await ExecuteDomainSubscriber(eventSerialized, eventDeserialized, subscriber, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger?.Error(e, e.Message);
                }
            }
        }

        private Task ExecuteDomainSubscriber(string body, DomainEvent domainEvent, string subscriber, CancellationToken cancellationToken)
        {
            return _retriever.ExecuteAsync<Task>(async ct => await _domainEventMediator.ExecuteOn(body, domainEvent, subscriber, ct),
                _ => true, cancellationToken);
        }
    }
}
