namespace BankAccounts.Domain.BankAccounts.Specifications;

internal class IsThePayroll : ISpecification<Movement>
{
    public Expression<Func<Movement, bool>> SatisfiedBy()
    {
        return movement => movement.Concept.ToLower().Contains("payroll")
                           || movement.Amount > 1_100;
    }
}