using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.ActiveMq.Events;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.ActiveMq;

[Collection("DockerHook")]
public class ActiveMqEventBusShould : EventBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Events/ActiveMq/appsettings.ActiveMq.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddActiveMqEventBus(Configuration);
    }

    [Fact]
    public async Task PublishDomainEventFromApacheMq()
    {
        await PublishDomainEvent();
    }
}
