namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    internal class InvalidIbanException : Exception
    {
        public InvalidIbanException() : base("Invalid Iban.") { }
    }
}
