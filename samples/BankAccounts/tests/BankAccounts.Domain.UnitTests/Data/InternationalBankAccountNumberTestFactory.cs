using BankAccounts.Domain.BankAccounts;
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace BankAccounts.Domain.Tests.Data
{
    internal static class InternationalBankAccountNumberTestFactory
    {
        public static Result<InternationalBankAccountNumber> Create(string? countryCheckDigit = default,
            string? entityCode = default, string? officeNumber = default, string? controlDigit = default,
            string? accountNumber = default)
        {
            return InternationalBankAccountNumber.Create(countryCheckDigit ?? "1111", entityCode ?? "2222",
                officeNumber ?? "333", controlDigit ?? "33", accountNumber ?? "4444444444");
        }
    }
}
