using Microsoft.Extensions.Options;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Mongo.Data;
using SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories;

public class SharedKernelMongoUnitOfWork : MongoUnitOfWorkAsync, ISharedKernelUnitOfWork
{
    public SharedKernelMongoUnitOfWork(IOptions<MongoSettings> options, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(options, auditableService, classValidatorService)
    {
    }
}