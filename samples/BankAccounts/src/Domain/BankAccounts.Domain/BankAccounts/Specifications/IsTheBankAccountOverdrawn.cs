namespace BankAccounts.Domain.BankAccounts.Specifications
{
    internal class IsTheBankAccountOverdrawn : ISpecification<BankAccount>
    {
        public Expression<Func<BankAccount, bool>> SatisfiedBy()
        {
            return bankAccount => bankAccount.Balance < 0;
        }
    }
}
