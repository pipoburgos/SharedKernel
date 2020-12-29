using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedKernel.Domain.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publish an event to event bus
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task Publish(DomainEvent @event, CancellationToken cancellationToken);

        /// <summary>
        /// Publish a list of events to event bus
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task Publish(List<DomainEvent> events, CancellationToken cancellationToken);
    }
}