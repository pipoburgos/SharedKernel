using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;
using BankAccounts.Infrastructure.Shared.Data;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Repositories;

namespace BankAccounts.Infrastructure.BankAccounts
{
    internal class EntityFrameworkBankAccountRepository :
        EntityFrameworkCoreRepositoryAsync<BankAccount>, IBankAccountRepository
    {
        public EntityFrameworkBankAccountRepository(BankAccountDbContext dbContext) : base(dbContext) { }
    }
}
