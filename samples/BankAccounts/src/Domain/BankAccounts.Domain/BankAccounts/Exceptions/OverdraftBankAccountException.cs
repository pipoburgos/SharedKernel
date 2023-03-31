namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    internal class OverdraftBankAccountException : Exception
    {
        public OverdraftBankAccountException() : base("You can't withdraw money from an overdraft account") { }
    }
}
