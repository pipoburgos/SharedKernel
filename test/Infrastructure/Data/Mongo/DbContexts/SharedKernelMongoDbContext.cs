using Microsoft.Extensions.Options;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Mongo.Data;
using SharedKernel.Infrastructure.Mongo.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Mongo.DbContexts;

public class SharedKernelMongoDbContext : MongoDbContext
{
    public SharedKernelMongoDbContext(IOptions<MongoSettings> options, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(options, auditableService, classValidatorService)
    {
    }
}