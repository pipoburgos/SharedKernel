using BankAccounts.Domain.BankAccounts.Events;
using System;

namespace BankAccounts.Domain.BankAccounts.Factory
{
    public static class BankAccountFactory
    {
        public static BankAccount Create(Guid id, Iban accountNumber, User owner, Movement initialMovement)
        {
            var bankAccount = new BankAccount(id, accountNumber, owner, initialMovement);

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }

        public static Movement CreateMovement(Guid id, string concept, decimal quantity, DateTime date)
        {
            return new Movement(id, concept, quantity, date);
        }

        public static User CreateUser(Guid id, string name, string surname, DateTime dateOfBirth)
        {
            return new User(id, name, surname, dateOfBirth);
        }
    }
}
