using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Integration.Tests.Events
{
    internal class SetCountWhenUserCreatedSubscriber : DomainEventSubscriber<UserCreated>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PublishUserCreatedDomainEvent _singletonValueContainer;

        public SetCountWhenUserCreatedSubscriber(
            IHttpContextAccessor httpContextAccessor,
            PublishUserCreatedDomainEvent singletonValueContainer)
        {
            _httpContextAccessor = httpContextAccessor;
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

            if (_httpContextAccessor?.HttpContext?.User.Claims.Any(e => e.Type == "Name" && e.Value == "Peter") == true)
                _singletonValueContainer.SumTotal();

            return Task.CompletedTask;
        }
    }
}