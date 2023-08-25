using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts
{
    internal class EntityFrameworkBankAccountRepository :
        EntityFrameworkCoreRepositoryAsync<BankAccount>, IBankAccountRepository
    {
        public EntityFrameworkBankAccountRepository(BankAccountDbContext dbContext) : base(dbContext) { }

        protected override IQueryable<BankAccount> GetAggregate(IQueryable<BankAccount> query)
        {
            return query.Include(t => t.Owner);
        }
    }
}
