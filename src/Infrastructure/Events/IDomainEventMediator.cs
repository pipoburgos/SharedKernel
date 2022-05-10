using SharedKernel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventMediator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="event"></param>
        /// <param name="eventSubscriber"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task ExecuteOn(string body, DomainEvent @event, Type eventSubscriber, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSerialized"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteDomainSubscribers(string eventSerialized, CancellationToken cancellationToken);
    }
}