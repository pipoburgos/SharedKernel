using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    internal class InMemoryDomainEventsConsumer
    {
        private readonly DomainEventsToExecute _domainEventsToExecute;
        private readonly DomainEventMediator _domainEventMediator;

        public InMemoryDomainEventsConsumer(
            DomainEventsToExecute domainEventsToExecute,
            DomainEventMediator domainEventMediator)
        {
            _domainEventsToExecute = domainEventsToExecute;
            _domainEventMediator = domainEventMediator;
        }

        public void Consume(DomainEvent @event)
        {
            var subscribers = DomainEventSubscriberInformationService.GetAllEventsSubscribers(@event);

            foreach (var subscriber in subscribers)
            {
                _domainEventsToExecute.Add(cancellationToken =>
                    _domainEventMediator.ExecuteOn(@event, subscriber, cancellationToken));
            }
            //return Task.WhenAll(subscribers.Select(subscriber =>
            //    _domainEventMediator.ExecuteOn(@event, subscriber, cancellationToken)));
        }
    }
}
