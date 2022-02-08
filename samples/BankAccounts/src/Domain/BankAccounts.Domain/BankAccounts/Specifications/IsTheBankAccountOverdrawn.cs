using SharedKernel.Domain.Specifications.Common;
using System;
using System.Linq.Expressions;

namespace BankAccounts.Domain.BankAccounts.Specifications
{
    public class IsTheBankAccountOverdrawn : ISpecification<BankAccount>
    {
        public Expression<Func<BankAccount, bool>> SatisfiedBy()
        {
            return bankAccount => bankAccount.Balance < 0;
        }
    }
}
