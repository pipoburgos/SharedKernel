using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.RabbitMq.Cqrs.Commands;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.RabbitMq;


public class RabbitMqCommandBusShould : CommandBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Cqrs/Commands/RabbitMq/appsettings.rabbitMq.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddSharedKernelRabbitMqCommandBusAsync(Configuration);
    }

    [Fact]
    public async Task DispatchCommandAsyncFromRabbitMq()
    {
        await DispatchCommandAsync();
    }
}
