using SharedKernel.Domain.Repositories;

namespace BankAccounts.Domain.BankAccounts.Repository
{
    public interface IBankAccountRepository : ICreateRepositoryAsync<BankAccount>,
        IUpdateRepositoryAsync<BankAccount>, IReadRepositoryAsync<BankAccount>
    {
    }
}
