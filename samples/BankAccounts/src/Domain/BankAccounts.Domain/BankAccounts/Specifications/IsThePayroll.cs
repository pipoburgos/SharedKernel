using SharedKernel.Domain.Specifications.Common;
using System;
using System.Linq.Expressions;

namespace BankAccounts.Domain.BankAccounts.Specifications
{
    public class IsThePayroll : ISpecification<Movement>
    {
        public Expression<Func<Movement, bool>> SatisfiedBy()
        {
            return movement => movement.Concept.ToLower().Contains("payroll")
                || movement.Amount > 1_100;
        }
    }
}
