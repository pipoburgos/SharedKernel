using BankAccounts.Domain.BankAccounts.Errors;
using BankAccounts.Domain.BankAccounts.Events;
using BankAccounts.Domain.BankAccounts.Specifications;

namespace BankAccounts.Domain.BankAccounts.Factories
{
    internal static class BankAccountFactory
    {
        public static Result<BankAccount> Create(Guid id, InternationalBankAccountNumber accountNumber, User owner,
            Movement initialMovement, DateTime now) =>
            Result
                .Create(owner)
                .Ensure(o => new AtLeast18YearsOldSpec(now).SatisfiedBy().Compile()(o),
                    Error.Create(BankAccountErrors.AtLeast18YearsOld, nameof(owner)))
                .Map(_ => new BankAccount(id, accountNumber, owner, initialMovement, now))
                .Tap(bankAccount => bankAccount.Record(new BankAccountCreated(bankAccount.Id.ToString())));
    }
}
