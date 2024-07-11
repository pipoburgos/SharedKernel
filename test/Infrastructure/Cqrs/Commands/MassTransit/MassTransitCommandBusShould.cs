using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.MassTransit;

public class MassTransitCommandBusShould : CommandBusCommonTestCase
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddMassTransitCommandBusAsync();
    }

    [Fact]
    public async Task DispatchCommandAsyncFromRabbitMq()
    {
        await DispatchCommandAsync(20);
    }
}
