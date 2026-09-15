using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Elasticsearch.Data;
using SharedKernel.Infrastructure.NetJson;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;
using SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch;

[Collection("DockerHook")]
public class ElasticsearchUserUnitOfWorkTests : UserUnitOfWorkTests<ElasticsearchUserRepository, ISharedKernelElasticsearchUnitOfWork>
{
    public override void BeforeStart()
    {
        WaitForElasticsearch();
        var db = GetRequiredService<SharedKernelElasticsearchDbContext>();
        db.DeleteIndexAsync<User>(CancellationToken.None).GetAwaiter().GetResult();
    }

    private void WaitForElasticsearch()
    {
        var client = GetRequiredService<ElasticsearchClient>();
        var timeout = TimeSpan.FromMinutes(2);
        var start = DateTime.UtcNow;

        while (DateTime.UtcNow - start < timeout)
        {
            try
            {
                var response = client.Cluster.Health();

                if (response.IsValidResponse)
                    return;
            }
            catch (Exception)
            {
                // Elasticsearch todavía no está disponible.
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        throw new TimeoutException(
            "Elasticsearch no estuvo disponible dentro del tiempo esperado.");
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernelElasticsearchUnitOfWork<ISharedKernelElasticsearchUnitOfWork, SharedKernelElasticsearchDbContext>(
                new Uri("http://admin:password@127.0.0.1:22228"),
                o => NetJsonSerializer.SetOptions(o, NamingConvention.SnakeCase))
            .AddSharedKernelNetJsonSerializer()
            .AddTransient<ElasticsearchUserRepository>();
    }
}
