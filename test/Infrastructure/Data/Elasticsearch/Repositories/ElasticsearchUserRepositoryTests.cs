using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Elasticsearch.Data;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

[Collection("DockerHook")]
public class ElasticsearchUserRepositoryTests : UserRepositoryCommonTestTests<ElasticsearchUserRepository>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddElasticsearchUnitOfWorkAsync<ISharedKernelUnitOfWork, SharedKernelUnitOfWork>(
                new Uri("http://localhost:22228"))
            .AddNewtonsoftSerializer()
            .AddTransient<ElasticsearchUserRepository>();
    }
}
