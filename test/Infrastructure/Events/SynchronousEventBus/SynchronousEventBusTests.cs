using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Events;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.SynchronousEventBus;

public class SynchronousEventBusTests : EventBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Events/SynchronousEventBus/appsettings.synchronous.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddSynchronousEventBus();
    }

    [Fact]
    public async Task PublishDomainEventFromSynchronousEventBus()
    {
        await PublishDomainEvent();
    }
}
