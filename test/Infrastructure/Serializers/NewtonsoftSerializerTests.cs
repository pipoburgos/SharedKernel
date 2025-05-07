using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Newtonsoft;

namespace SharedKernel.Integration.Tests.Serializers;

public class NewtonsoftSerializerTests : JsonSerializerTests
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddSharedKernelNewtonsoftSerializer();
    }
}
