using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Elasticsearch.Data;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;
using SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch;

[Collection("DockerHook")]
public class ElasticsearchUserRepositoryTests : UserRepositoryCommonTestTests<ElasticsearchUserRepository>
{
    public override void BeforeStart()
    {
        var db = GetRequiredService<SharedKernelElasticsearchDbContext>();

        db.DeleteIndexAsync<User>(CancellationToken.None).GetAwaiter().GetResult();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernelElasticsearchDbContext<SharedKernelElasticsearchDbContext>(
                new Uri("http://admin:password@127.0.0.1:22228"))
            .AddSharedKernelNewtonsoftSerializer()
            .AddTransient<ElasticsearchUserRepository>();
    }
}
