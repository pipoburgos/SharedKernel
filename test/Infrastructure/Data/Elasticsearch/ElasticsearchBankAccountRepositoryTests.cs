using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Elasticsearch.Data;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;
using SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch;

[Collection("DockerHook")]
public class ElasticsearchBankAccountRepositoryTests : BankAccountRepositoryCommonTestTests<ElasticsearchBankAccountRepository>
{
    public override void BeforeStart()
    {
        var db = GetRequiredService<SharedKernelElasticsearchDbContext>();

        db.Client.Indices.Delete("user");
        db.Client.Indices.Delete("bankaccount");
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddElasticsearchDbContext<SharedKernelElasticsearchDbContext>(
                new Uri("http://admin:password@127.0.0.1:22228"))
            .AddNewtonsoftSerializer()
            .AddTransient<ElasticsearchBankAccountRepository>();
    }
}
