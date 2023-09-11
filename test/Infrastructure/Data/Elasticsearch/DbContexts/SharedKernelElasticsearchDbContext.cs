using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;

public class SharedKernelElasticsearchDbContext : ElasticsearchDbContext
{
    public SharedKernelElasticsearchDbContext(ElasticLowLevelClient client, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(client,
        jsonSerializer, auditableService, classValidatorService)
    {
    }
}
