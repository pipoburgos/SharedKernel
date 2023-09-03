using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Data.FileSystem.Repositories;

namespace SharedKernel.Integration.Tests.Data.FileSystem;

public class FileSystemBankAccountRepository : FileSystemRepository<BankAccount, Guid>
{
    public FileSystemBankAccountRepository(SharedKernelUnitOfWork unitOfWork, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWork, configuration, jsonSerializer)
    {
    }
}
