using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Integration.Tests.Events
{
    internal class SetUserIdWhenUserCreatedSubscriber : IDomainEventSubscriber<UserCreated>
    {
        private readonly PublishUserCreatedDomainEvent _publishUserCreatedDomainEvent;

        public SetUserIdWhenUserCreatedSubscriber(
            PublishUserCreatedDomainEvent publishUserCreatedDomainEvent)
        {
            _publishUserCreatedDomainEvent = publishUserCreatedDomainEvent;
        }


        public Task On(UserCreated @event, CancellationToken cancellationToken)
        {
            if (@event == default)
                throw new ArgumentNullException(nameof(@event));

            _publishUserCreatedDomainEvent.SetUser(@event.Id);
            return Task.CompletedTask;
        }
    }
}