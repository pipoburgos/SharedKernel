namespace BankAccounts.Domain.BankAccounts.Repository
{
    internal interface IBankAccountRepository : ICreateRepositoryAsync<BankAccount>,
        IUpdateRepositoryAsync<BankAccount>, IReadRepositoryAsync<BankAccount>
    {
    }
}
