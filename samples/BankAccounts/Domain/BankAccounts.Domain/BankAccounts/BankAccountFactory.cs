using System;

namespace BankAccounts.Domain.BankAccounts
{
    public static class BankAccountFactory
    {
        public static BankAccount Create(Guid id, AccountNumber accountNumber, User owner, Movement initialMovement)
        {
            var bankAccount = new BankAccount(id, accountNumber, owner, initialMovement);

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }

        public static Movement CreateMovement(Guid id, string concept, decimal quantity, DateTime date)
        {
            return new Movement(id, concept, quantity, date);
        }
    }
}
