namespace BankAccounts.Domain.BankAccounts.Specifications
{
    internal class IsASpanishBankAccountSpec : ISpecification<BankAccount>
    {
        public Expression<Func<BankAccount, bool>> SatisfiedBy()
        {
            return bankAccount => bankAccount.InternationalBankAccountNumber.CountryCheckDigit.ToUpper().StartsWith("ES");
        }
    }
}
