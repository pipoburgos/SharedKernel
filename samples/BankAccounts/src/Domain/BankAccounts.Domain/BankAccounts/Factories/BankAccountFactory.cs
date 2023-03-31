using BankAccounts.Domain.BankAccounts.Events;

namespace BankAccounts.Domain.BankAccounts.Factories
{
    internal static class BankAccountFactory
    {
        public static BankAccount Create(Guid id, InternationalBankAccountNumber accountNumber, User owner, Movement initialMovement, DateTime now)
        {
            var bankAccount = new BankAccount(id, accountNumber, owner, initialMovement, now);

            bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString()));

            return bankAccount;
        }
    }
}
