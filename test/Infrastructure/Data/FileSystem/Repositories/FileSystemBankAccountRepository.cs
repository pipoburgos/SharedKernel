using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.FileSystem.Data.Repositories;
using SharedKernel.Integration.Tests.Data.FileSystem.DbContexts;

namespace SharedKernel.Integration.Tests.Data.FileSystem.Repositories;

public class FileSystemBankAccountRepository : FileSystemRepository<BankAccount, Guid>
{
    public FileSystemBankAccountRepository(SharedKernelFileSystemDbContext unitOfWork, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWork, configuration, jsonSerializer)
    {
    }
}
