using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.NetJson;

namespace SharedKernel.Integration.Tests.Serializers;

public class NetJsonSerializerTests : JsonSerializerTests
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddSharedKernelNetJsonSerializer();
    }
}
