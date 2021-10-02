using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    internal class InMemoryDomainEventsConsumer
    {
        private readonly DomainEventsToExecute _domainEventsToExecute;
        private readonly DomainEventMediator _domainEventMediator;
        private readonly DomainEventJsonSerializer _serializer;
        private readonly DomainEventJsonDeserializer _deserializer;

        public InMemoryDomainEventsConsumer(
            DomainEventsToExecute domainEventsToExecute,
            DomainEventMediator domainEventMediator,
            DomainEventJsonSerializer serializer,
            DomainEventJsonDeserializer deserializer)
        {
            _domainEventsToExecute = domainEventsToExecute;
            _domainEventMediator = domainEventMediator;
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public void Consume(DomainEvent @event)
        {
            var subscribers = DomainEventSubscriberInformationService.GetAllEventsSubscribers(@event);

            var eventSerialized = _serializer.Serialize(@event);
            var eventDeserialized = _deserializer.Deserialize(eventSerialized);
            foreach (var subscriber in subscribers)
            {
                _domainEventsToExecute.Add(cancellationToken =>
                    _domainEventMediator.ExecuteOn(eventDeserialized, subscriber, cancellationToken));
            }
        }
    }
}
