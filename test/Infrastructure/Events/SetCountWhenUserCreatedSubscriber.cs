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

            var rnd = new Random();
            var random = rnd.Next(1, 15);

            if (random == 2)
                throw new Exception("To retry");

            _singletonValueContainer.SumTotal();

            return Task.CompletedTask;
        }
    }
}