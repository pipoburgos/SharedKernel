using SharedKernel.Application.Events;
using SharedKernel.Application.Security;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Integration.Tests.Events
{
    internal class SetCountWhenUserCreatedSubscriber : IDomainEventSubscriber<UserCreated>
    {
        private readonly IIdentityService _httpContextAccessor;
        private readonly PublishUserCreatedDomainEvent _publishUserCreatedDomainEvent;

        public SetCountWhenUserCreatedSubscriber(
            IIdentityService httpContextAccessor,
            PublishUserCreatedDomainEvent publishUserCreatedDomainEvent)
        {
            _httpContextAccessor = httpContextAccessor;
            _publishUserCreatedDomainEvent = publishUserCreatedDomainEvent;
        }


        public Task On(UserCreated @event, CancellationToken cancellationToken)
        {
            if (@event == default)
                throw new ArgumentNullException(nameof(@event));

            var rnd = new Random();
            var random = rnd.Next(1, 7);

            if (random == 1)
                throw new Exception("To retry");

            if (_httpContextAccessor.User.Claims.Any(e => e.Type == "Name" && e.Value == "Peter"))
                _publishUserCreatedDomainEvent.SumTotal();

            return Task.CompletedTask;
        }
    }
}