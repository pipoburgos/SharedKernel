using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Mongo.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories;

[Collection("DockerHook")]
public class MongoRepositoryCreateTests : RepositoryCommonTestTests<UserMongoRepository>
{
    protected override string GetJsonFile()
    {
        return "Data/Mongo/appsettings.mongo.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddMongoUnitOfWorkAsync<ISharedKernelUnitOfWork, SharedKernelMongoUnitOfWork>(Configuration)
            .AddTransient<UserMongoRepository>();
    }
}
