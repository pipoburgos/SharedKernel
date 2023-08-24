namespace BankAccounts.Domain.BankAccounts.Errors;
internal class BankAccountErrors
{
    public const string AtLeast18YearsOld = "At Least 18 Years Old";
    public const string InvalidIban = "Invalid Iban.";
    public const string OverdraftBankAccount = "You can't withdraw money from an overdraft account";
    public const string QuantityCannotBeNegative = "Quantity cannot be negative.";
}
