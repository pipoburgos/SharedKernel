using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Events;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.ApacheActiveMq;

[Collection("DockerHook")]
public class ApacheActiveMqEventBusShould : EventBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Events/ApacheActiveMq/appsettings.Apache.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddApacheActiveMqEventBus(Configuration);
    }

    [Fact]
    public async Task PublishDomainEventFromApacheMq()
    {
        await PublishDomainEvent();
    }
}
