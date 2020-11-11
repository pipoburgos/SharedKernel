using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Integration.Tests.Events.RabbitMq
{
    internal class TestAllWorksOnRabbitMqEventsPublishedSubscriber : DomainEventSubscriber<UserCreated>
    {
        private readonly SingletonValueContainer _singletonValueContainer;

        public TestAllWorksOnRabbitMqEventsPublishedSubscriber(
            SingletonValueContainer singletonValueContainer)
        {
            _singletonValueContainer = singletonValueContainer;
        }


        protected override Task On(UserCreated @event, CancellationToken cancellationToken)
        {
            if (@event == default)
                throw new ArgumentNullException(nameof(@event));

            _singletonValueContainer.UserId = @event.Id;
            return Task.CompletedTask;
        }
    }
}