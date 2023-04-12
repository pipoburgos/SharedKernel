using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.Security;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Integration.Tests.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.InMemory
{
    public class InMemoryEventBusTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Events/InMemory/appsettings.inMemory.json";
        }

        /// <summary>RetryPolicyMiddleware Exception handler</summary>
        public class RetryPolicyExceptionHandler : IRetryPolicyExceptionHandler
        {
            /// <summary>Check if an exception has to be retried</summary>
            /// <param name="exception"></param>
            /// <returns></returns>
            public bool NeedToRetryTheException(Exception exception) => true;
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddInMemoryEventBus(Configuration)
                .AddDomainEvents(typeof(UserCreated))
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>()

                .AddTransient(typeof(IMiddleware<>), typeof(ValidationMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>))

                .AddTransient<IRetriever, PollyRetriever>()
                .AddTransient<IRetryPolicyExceptionHandler, RetryPolicyExceptionHandler>()
                .AddPollyRetry(Configuration)

                .AddTransient(typeof(IMiddleware<>), typeof(RetryPolicyMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(RetryPolicyMiddleware<,>))


                .RemoveAll<IIdentityService>()
                .AddScoped<IIdentityService, HttpContextAccessorIdentityService>()
                .AddHttpContextAccessor();
        }

        [Fact]
        public async Task PublishDomainEventFromMemory()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(this);
        }
    }
}
