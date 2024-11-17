using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.MassTransit.Events;

namespace SharedKernel.Integration.Tests.Events.MassTransit;


public class MassTransitEventBusShould : EventBusCommonTestCase
{

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddSharedKernelMassTransitEventBus(x => x.UsingInMemory((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        }));
    }

    [Fact]
    public async Task PublishDomainEventFromMassTransit()
    {
        await PublishDomainEvent();
    }
}