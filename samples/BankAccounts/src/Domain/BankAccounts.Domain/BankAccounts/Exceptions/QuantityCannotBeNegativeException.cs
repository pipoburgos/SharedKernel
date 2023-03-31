namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    internal class QuantityCannotBeNegativeException : Exception
    {
        public QuantityCannotBeNegativeException() : base("Quantity cannot be negative.") { }
    }
}
