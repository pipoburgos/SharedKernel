using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.Security;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Testing.Infrastructure;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.ApacheActiveMq
{
    [Collection("DockerHook")]
    public class ApacheActiveMqEventBusShould : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Events/ApacheActiveMq/appsettings.Apache.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddApacheActiveMq(Configuration)
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber), typeof(UserCreated))
                .AddNetJsonSerializer()
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
        public async Task PublishDomainEventFromApacheMq()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(this);
        }
    }
}