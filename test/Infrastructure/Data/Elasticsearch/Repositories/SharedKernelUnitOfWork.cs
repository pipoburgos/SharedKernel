using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public interface ISharedKernelUnitOfWork2 : IDbContextAsync { }

public class SharedKernelUnitOfWork2 : ElasticsearchDbContext, ISharedKernelUnitOfWork2
{
    public SharedKernelUnitOfWork2(ElasticLowLevelClient client, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(client,
        jsonSerializer, auditableService, classValidatorService)

    {
    }
}
