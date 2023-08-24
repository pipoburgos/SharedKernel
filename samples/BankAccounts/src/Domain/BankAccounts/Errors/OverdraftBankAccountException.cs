namespace BankAccounts.Domain.BankAccounts.Errors
{
    internal class OverdraftBankAccountException : Exception
    {
        public OverdraftBankAccountException() : base(BankAccountErrors.OverdraftBankAccount) { }
    }
}
