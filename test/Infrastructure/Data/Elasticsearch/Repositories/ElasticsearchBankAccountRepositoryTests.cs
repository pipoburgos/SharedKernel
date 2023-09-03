using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Elasticsearch.Data;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

[Collection("DockerHook")]
public class ElasticsearchBankAccountRepositoryTests : BankAccountRepositoryCommonTestTests<ElasticsearchBankAccountRepository>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddElasticsearchUnitOfWorkAsync<ISharedKernelUnitOfWork, SharedKernelUnitOfWork>(
                new Uri("http://127.0.0.1:22228"))
            .AddNewtonsoftSerializer()
            .AddTransient<ElasticsearchBankAccountRepository>();
    }
}
