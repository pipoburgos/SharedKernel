using System;

namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    public class OverdraftBankAccountException : Exception
    {
        public OverdraftBankAccountException() : base("You can't withdraw money from an overdraft account") { }
    }
}
