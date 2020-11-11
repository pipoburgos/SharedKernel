using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Events
{
    public abstract class DomainEventSubscriber<TDomain> : IDomainEventSubscriberBase where TDomain : DomainEvent
    {
        public Task On(DomainEvent @event, CancellationToken cancellationToken)
        {
            if (@event is TDomain msg)
                return On(msg, cancellationToken);

            return TaskHelper.CompletedTask;
        }

        protected abstract Task On(TDomain @event, CancellationToken cancellationToken);
    }
}