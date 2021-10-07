using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Integration.Tests.Shared;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Integration.Tests.Events
{
    public static class PublishUserCreatedDomainEventCase
    {
        public static async Task PublishDomainEvent(InfrastructureTestCase testCase)
        {
            var httpContextAccessor = testCase.GetRequiredService<IHttpContextAccessor>();

            if (httpContextAccessor != default)
            {
#if !NET461 && !NETSTANDARD
                httpContextAccessor.HttpContext ??= new DefaultHttpContext();
#endif
                httpContextAccessor.HttpContext.User =
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("Name", "Peter") }));
            }

            var user1 = UserMother.Create();
            var user2 = UserMother.Create();
            var user3 = UserMother.Create();
            var user4 = UserMother.Create();

            var domainEvents = user1.PullDomainEvents();
            domainEvents.AddRange(user2.PullDomainEvents());
            domainEvents.AddRange(user3.PullDomainEvents());
            domainEvents.AddRange(user4.PullDomainEvents());

            var eventBus = testCase.GetRequiredService<IEventBus>();
            var tasks = new List<Task>();
            const int total = 5;
            for (var i = 0; i < total; i++)
            {
                tasks.Add(eventBus.Publish(domainEvents, CancellationToken.None));
            }

            await Task.WhenAll(tasks);

            // Esperar a que terminen los manejadores de los eventos junto con la política de reintentos
            await Task.Delay(5_000, CancellationToken.None);

            var singletonValueContainer = testCase.GetRequiredService<PublishUserCreatedDomainEvent>();
            singletonValueContainer.Total.Should().Be(total * 4);

            singletonValueContainer.Users.Should().Contain(u => u == user1.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user2.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user3.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user4.Id);
        }
    }
}
