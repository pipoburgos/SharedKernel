using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Cqrs.Commands;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.ApacheActiveMq;

[Collection("DockerHook")]
public class ApacheActiveMqCommandBusShould : CommandBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Cqrs/Commands/ApacheActiveMq/appsettings.Apache.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddApacheActiveMqCommandBusAsync(Configuration);
    }

    [Fact]
    public async Task DispatchCommandAsyncFromApacheMq()
    {
        await DispatchCommandAsync();
    }
}
