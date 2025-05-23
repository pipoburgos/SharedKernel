using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.RabbitMq.Events;

namespace SharedKernel.Integration.Tests.Events.RabbitMq;


public class RabbitMqEventBusShould : EventBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Events/RabbitMq/appsettings.rabbitMq.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddSharedKernelRabbitMqEventBus(Configuration);
    }

    [Fact]
    public async Task PublishDomainEventFromRabbitMq()
    {
        await PublishDomainEvent();
    }
}