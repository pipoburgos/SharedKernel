using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Cqrs.Commands;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.RabbitMq;

[Collection("DockerHook")]
public class RedisCommandBusShould : CommandBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Cqrs/Commands/RabbitMq/appsettings.rabbitMq.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddRabbitMqCommandBusAsync(Configuration);
    }

    [Fact]
    public async Task DispatchCommandAsyncFromRabbitMq()
    {
        await DispatchCommandAsync();
    }
}
