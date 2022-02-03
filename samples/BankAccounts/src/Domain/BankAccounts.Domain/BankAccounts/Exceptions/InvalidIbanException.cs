using System;

namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    public class InvalidIbanException : Exception
    {
        public InvalidIbanException() : base("Invalid Iban.") { }
    }
}
