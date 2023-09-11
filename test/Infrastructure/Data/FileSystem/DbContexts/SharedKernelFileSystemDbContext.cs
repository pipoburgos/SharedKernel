using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.FileSystem.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.FileSystem.DbContexts;

public class SharedKernelFileSystemDbContext : FileSystemDbContext, ISharedKernelFileSystemUnitOfWork
{
    public SharedKernelFileSystemDbContext(IConfiguration configuration, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(configuration,
        jsonSerializer, auditableService, classValidatorService)
    {
    }
}
