using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.RabbitMq
{
    [Collection("DockerHook")]
    public class RabbitMqEventBusShould : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Events/RabbitMq/appsettings.rabbitMq.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddDomainEvents(typeof(UserCreated))
                .AddRabbitMqEventBus(Configuration)
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>()
                .AddHttpContextAccessor()

                .AddTransient(typeof(IMiddleware<>), typeof(ValidationMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>))

                .AddTransient<IRetriever, PollyRetriever>()
                .AddTransient<IRetryPolicyExceptionHandler, RetryPolicyExceptionHandler>()
                .AddPollyRetry(Configuration)

                .AddTransient(typeof(IMiddleware<>), typeof(RetryPolicyMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(RetryPolicyMiddleware<,>));
        }

        [Fact]
        public async Task PublishDomainEventFromRabbitMq()
        {
            await Task.Delay(7_000);
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(this);
        }
    }
}