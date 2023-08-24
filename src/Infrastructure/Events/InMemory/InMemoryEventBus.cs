using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// In memory event bus
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private readonly IInMemoryDomainEventsConsumer _domainEventsToExecute;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domainEventsToExecute"></param>
        public InMemoryEventBus(
            IInMemoryDomainEventsConsumer domainEventsToExecute)
        {
            _domainEventsToExecute = domainEventsToExecute;
        }

        /// <summary>
        /// Publish an event to event bus
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return Publish(new List<DomainEvent> { @event }, cancellationToken);
        }

        /// <summary>
        /// Publish an event to event bus
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            if (events == default)
                return Task.CompletedTask;

            _domainEventsToExecute.AddRange(events);

            return Task.CompletedTask;
        }
    }
}