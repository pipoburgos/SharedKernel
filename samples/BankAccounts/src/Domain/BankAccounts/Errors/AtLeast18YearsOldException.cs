namespace BankAccounts.Domain.BankAccounts.Errors
{
    internal class AtLeast18YearsOldException : Exception
    {
        public AtLeast18YearsOldException() : base(BankAccountErrors.AtLeast18YearsOld) { }
    }
}
