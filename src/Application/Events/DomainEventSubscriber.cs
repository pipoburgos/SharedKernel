using SharedKernel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public abstract class DomainEventSubscriber<TDomain> : IDomainEventSubscriberBase where TDomain : DomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task On(DomainEvent @event, CancellationToken cancellationToken)
        {
            if (@event is TDomain msg)
                return On(msg, cancellationToken);

            throw new ArgumentException($"Unable cast {@event.GetType()} to {typeof(TDomain)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        protected abstract Task On(TDomain @event, CancellationToken cancellationToken);
    }
}