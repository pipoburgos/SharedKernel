using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.FileSystem.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.FileSystem;

public class FileSystemBankAccountRepository : FileSystemRepositoryAsync<BankAccount, Guid>
{
    public FileSystemBankAccountRepository(SharedKernelUnitOfWork unitOfWork, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWork, configuration, jsonSerializer)
    {
    }
}
