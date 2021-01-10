using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Infraestructure.Tests.Events
{
    internal class SetUserIdWhenUserCreatedSubscriber : DomainEventSubscriber<UserCreated>
    {
        private readonly PublishUserCreatedDomainEvent _singletonValueContainer;

        public SetUserIdWhenUserCreatedSubscriber(
            PublishUserCreatedDomainEvent singletonValueContainer)
        {
            _singletonValueContainer = singletonValueContainer;
        }


        protected override Task On(UserCreated @event, CancellationToken cancellationToken)
        {
            if (@event == default)
                throw new ArgumentNullException(nameof(@event));

            _singletonValueContainer.SetUser(@event.Id);
            return Task.CompletedTask;
        }
    }
}