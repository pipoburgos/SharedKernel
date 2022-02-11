using BankAccounts.Domain.BankAccounts;

namespace BankAccounts.Domain.UnitTests.Data
{
    public static class InternationalBankAccountNumberTestFactory
    {
        public static InternationalBankAccountNumber Create(string countryCheckDigit = default,
            string entityCode = default, string officeNumber = default, string controlDigit = default,
            string accountNumber = default)
        {
            return new InternationalBankAccountNumber(countryCheckDigit ?? "1111", entityCode ?? "2222",
                officeNumber ?? "333", controlDigit ?? "33", accountNumber ?? "4444444444");
        }
    }
}
