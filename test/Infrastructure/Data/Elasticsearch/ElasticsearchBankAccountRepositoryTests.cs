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

        db.Client.Indices.DeleteAsync("user").GetAwaiter().GetResult();
        db.Client.Indices.DeleteAsync("bankaccount").GetAwaiter().GetResult();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernelElasticsearchDbContext<SharedKernelElasticsearchDbContext>(
                new Uri("http://admin:password@127.0.0.1:22228"))
            .AddSharedKernelNewtonsoftSerializer()
            .AddTransient<ElasticsearchBankAccountRepository>();
    }
}
