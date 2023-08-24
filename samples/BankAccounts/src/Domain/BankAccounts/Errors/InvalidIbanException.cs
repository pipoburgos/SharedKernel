namespace BankAccounts.Domain.BankAccounts.Errors
{
    internal class InvalidIbanException : Exception
    {
        public InvalidIbanException() : base(BankAccountErrors.InvalidIban) { }
    }
}
