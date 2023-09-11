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
            .AddElasticsearchUnitOfWork<ISharedKernelUnitOfWork2, SharedKernelUnitOfWork2>(
                new Uri("http://admin:password@127.0.0.1:22228"))
            .AddNewtonsoftSerializer()
            .AddTransient<ElasticsearchUserRepository>();
    }
}
