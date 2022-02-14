using System;

namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    public class AtLeast18YearsOldException : Exception
    {
        public AtLeast18YearsOldException() : base("At Least 18 Years OldException") { }
    }
}
