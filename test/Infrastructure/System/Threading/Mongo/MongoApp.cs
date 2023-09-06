using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Mongo.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.Mongo;

public class MongoApp : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "System/Threading/Mongo/appsettings.mongo.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services.AddMongoMutex(Configuration);
    }
}
