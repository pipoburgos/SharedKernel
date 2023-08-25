using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts;

internal class EntityFrameworkBankAccountRepository :
    EntityFrameworkCoreRepositoryAsync<BankAccount, BankAccountId, BankAccountDbContext>, IBankAccountRepository
{
    public EntityFrameworkBankAccountRepository(BankAccountDbContext dbContextBase,
        IDbContextFactory<BankAccountDbContext> dbContextFactory) : base(dbContextBase, dbContextFactory)
    {
    }

    protected override IQueryable<BankAccount> GetAggregate(IQueryable<BankAccount> query)
    {
        return query.Include(t => t.Owner);
    }
}
