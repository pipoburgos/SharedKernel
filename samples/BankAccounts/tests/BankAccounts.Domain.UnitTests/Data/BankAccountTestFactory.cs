using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Events;
using System;

namespace BankAccounts.Domain.UnitTests.Data
{
    public static class BankAccountTestFactory
    {
        public static BankAccount Create(Guid? id = default, InternationalBankAccountNumber iban = default,
            User owner = default, Movement initialMovement = default, string countryCheckDigit = default,
            string entityCode = default, string officeNumber = default, string controlDigit = default,
            string accountNumber = default, decimal? amount = default)
        {
            var bankAccount = new BankAccount(id ?? Guid.NewGuid(),
                iban ?? new InternationalBankAccountNumber(countryCheckDigit ?? "1111", entityCode ?? "2222",
                    officeNumber ?? "333", controlDigit ?? "33", accountNumber ?? "4444444444"),
                owner ?? new User(Guid.NewGuid(), "ABC", "SUR", new DateTime(1980, 2, 25)),
                initialMovement ?? new Movement(Guid.NewGuid(), "Conce", amount ?? 23, new DateTime(2020, 3, 5)), new DateTime(2020, 3, 5));

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }
    }
}
