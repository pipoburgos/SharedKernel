using BankAccounts.Domain.BankAccounts.Events;
using System;

namespace BankAccounts.Domain.BankAccounts.Factories
{
    public static class BankAccountFactory
    {
        public static BankAccount Create(Guid id, InternationalBankAccountNumber accountNumber, User owner, Movement initialMovement)
        {
            var bankAccount = new BankAccount(id, accountNumber, owner, initialMovement);

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }
    }
}
