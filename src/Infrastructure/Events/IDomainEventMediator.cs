using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

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
        Task ExecuteOn(string body, DomainEvent @event, string eventSubscriber, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSerialized"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteDomainSubscribers(string eventSerialized, CancellationToken cancellationToken);
    }
}