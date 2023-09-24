using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.ActiveMq.Cqrs.Comamnds;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.ActiveMq;

[Collection("DockerHook")]
public class ActiveMqCommandBusShould : CommandBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Cqrs/Commands/ActiveMq/appsettings.ActiveMq.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddActiveMqCommandBusAsync(Configuration);
    }

    [Fact]
    public async Task DispatchCommandAsyncFromActiveMq()
    {
        await DispatchCommandAsync();
    }
}
