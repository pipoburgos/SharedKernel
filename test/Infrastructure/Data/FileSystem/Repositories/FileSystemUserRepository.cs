using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.FileSystem.Data.Repositories;
using SharedKernel.Integration.Tests.Data.FileSystem.DbContexts;

namespace SharedKernel.Integration.Tests.Data.FileSystem.Repositories;

public class FileSystemUserRepository : FileSystemRepository<User, Guid>
{
    public FileSystemUserRepository(SharedKernelFileSystemDbContext unitOfWork, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWork, configuration, jsonSerializer)
    {
    }
}
