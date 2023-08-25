namespace BankAccounts.Domain.BankAccounts.Repository
{
    internal interface IBankAccountRepository : ICreateRepositoryAsync<BankAccount>, IReadRepositoryAsync<BankAccount, BankAccountId>
    {
    }
}
