using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Integration.Tests.Events
{
    internal class SetCountWhenUserCreatedSubscriber : DomainEventSubscriber<UserCreated>
    {
        private readonly PublishUserCreatedDomainEvent _singletonValueContainer;

        public SetCountWhenUserCreatedSubscriber(
            PublishUserCreatedDomainEvent singletonValueContainer)
        {
            _singletonValueContainer = singletonValueContainer;
        }


        protected override Task On(UserCreated @event, CancellationToken cancellationToken)
        {
            if (@event == default)
                throw new ArgumentNullException(nameof(@event));

            _singletonValueContainer.SumTotal();

            return Task.CompletedTask;
        }
    }
}