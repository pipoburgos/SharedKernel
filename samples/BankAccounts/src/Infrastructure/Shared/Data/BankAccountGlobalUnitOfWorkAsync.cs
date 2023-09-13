using BankAccounts.Application.Shared.UnitOfWork;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace BankAccounts.Infrastructure.Shared.Data;

public class BankAccountGlobalUnitOfWorkAsync : GlobalUnitOfWorkAsync, IBankAccountUnitOfWork
{
    public BankAccountGlobalUnitOfWorkAsync(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}