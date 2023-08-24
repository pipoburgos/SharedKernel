namespace BankAccounts.Domain.BankAccounts.Errors
{
    internal class QuantityCannotBeNegativeException : Exception
    {
        public QuantityCannotBeNegativeException() : base(BankAccountErrors.QuantityCannotBeNegative) { }
    }
}
