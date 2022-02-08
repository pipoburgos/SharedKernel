using System;

namespace BankAccounts.Domain.BankAccounts.Exceptions
{
    public class QuantityCannotBeNegativeException : Exception
    {
        public QuantityCannotBeNegativeException() : base("Quantity cannot be negative.") { }
    }
}
