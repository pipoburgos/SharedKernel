using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventSubscriberBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task On(DomainEvent @event, CancellationToken cancellationToken);
    }
}