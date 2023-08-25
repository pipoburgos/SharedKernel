using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Events;

namespace BankAccounts.Domain.Tests.Data;

internal static class BankAccountTestFactory
{
    public static BankAccount Create(Guid? id = default, InternationalBankAccountNumber iban = default,
        User owner = default, Movement initialMovement = default)
    {
        var bankAccount = BankAccount.Create(id ?? Guid.NewGuid(),
            iban ?? InternationalBankAccountNumberTestFactory.Create().Value, owner ?? UserTestFactory.Create().Value,
            initialMovement ?? MovementTestFactory.Create().Value, DateTime.Now).Value;

        bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

        return bankAccount;
    }
}
