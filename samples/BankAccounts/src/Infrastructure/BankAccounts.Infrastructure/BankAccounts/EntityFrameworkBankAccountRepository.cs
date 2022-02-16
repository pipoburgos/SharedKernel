using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccounts.Infrastructure.BankAccounts
{
    internal class EntityFrameworkBankAccountRepository : IBankAccountRepository
    {
        public async Task Add(BankAccount bankAccount, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task Update(BankAccount bankAccount, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<BankAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
