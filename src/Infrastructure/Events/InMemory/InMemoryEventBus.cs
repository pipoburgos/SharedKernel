using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// In memory event bus
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private readonly IDomainEventsToExecute _domainEventsToExecute;
        private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domainEventsToExecute"></param>
        /// <param name="executeMiddlewaresService"></param>
        public InMemoryEventBus(
            IDomainEventsToExecute domainEventsToExecute,
            IExecuteMiddlewaresService executeMiddlewaresService)
        {
            _domainEventsToExecute = domainEventsToExecute;
            _executeMiddlewaresService = executeMiddlewaresService;
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
        public async Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            if (events == default)
                return;

            foreach (var domainEvent in events)
            {
                await _executeMiddlewaresService.ExecuteAsync(domainEvent, cancellationToken, (@event, _) =>
                {
                    _domainEventsToExecute.Add(@event);
                    return Task.CompletedTask;
                });
            }
        }
    }
}