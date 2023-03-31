namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    internal class AtLeast18YearsOldException : Exception
    {
        public AtLeast18YearsOldException() : base("At Least 18 Years OldException") { }
    }
}
