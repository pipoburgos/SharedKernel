using BankAccounts.Domain.BankAccounts.Events;
using System;

namespace BankAccounts.Domain.BankAccounts.Factories
{
    public static class BankAccountFactory
    {
        public static BankAccount Create(Guid id, InternationalBankAccountNumber accountNumber, User owner, Movement initialMovement, DateTime now)
        {
            var bankAccount = new BankAccount(id, accountNumber, owner, initialMovement, now);

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }
    }
}
