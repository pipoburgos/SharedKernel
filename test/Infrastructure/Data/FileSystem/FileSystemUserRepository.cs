using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.FileSystem.Repositories;

namespace SharedKernel.Integration.Tests.Data.FileSystem;

public class FileSystemUserRepository : FileSystemRepositoryAsync<User, Guid>
{
    public FileSystemUserRepository(SharedKernelUnitOfWork unitOfWork, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWork, configuration, jsonSerializer)
    {
    }
}
