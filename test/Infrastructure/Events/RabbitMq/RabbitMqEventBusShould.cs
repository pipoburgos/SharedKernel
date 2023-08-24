using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.RabbitMq.Events;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.RabbitMq;

[Collection("DockerHook")]
public class RabbitMqEventBusShould : EventBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Events/RabbitMq/appsettings.rabbitMq.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddRabbitMqEventBus(Configuration);
    }

    [Fact]
    public async Task PublishDomainEventFromRabbitMq()
    {
        await PublishDomainEvent();
    }
}