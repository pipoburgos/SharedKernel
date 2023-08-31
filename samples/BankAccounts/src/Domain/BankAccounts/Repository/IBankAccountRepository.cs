using SharedKernel.Domain.Repositories.Create;
using SharedKernel.Domain.Repositories.Read;

namespace BankAccounts.Domain.BankAccounts.Repository;

internal interface IBankAccountRepository :
    ICreateRepositoryAsync<BankAccount>,
    IReadOneRepositoryAsync<BankAccount, BankAccountId>
{
}
