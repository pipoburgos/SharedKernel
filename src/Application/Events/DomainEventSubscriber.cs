using SharedKernel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Events
{
    public abstract class DomainEventSubscriber<TDomain> : IDomainEventSubscriberBase where TDomain : DomainEvent
    {
        public Task On(DomainEvent @event, CancellationToken cancellationToken)
        {
            if (@event is TDomain msg)
                return On(msg, cancellationToken);

            throw new ArgumentException(nameof(@event));
        }

        protected abstract Task On(TDomain @event, CancellationToken cancellationToken);
    }
}