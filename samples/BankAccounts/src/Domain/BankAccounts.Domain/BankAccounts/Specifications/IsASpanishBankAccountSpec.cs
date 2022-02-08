using SharedKernel.Domain.Specifications.Common;
using System;
using System.Linq.Expressions;

namespace BankAccounts.Domain.BankAccounts.Specifications
{
    public class IsASpanishBankAccountSpec : ISpecification<BankAccount>
    {
        public Expression<Func<BankAccount, bool>> SatisfiedBy()
        {
            return bankAccount => bankAccount.InternationalBankAccountNumber.CountryCheckDigit.ToUpper().StartsWith("ES");
        }
    }
}
