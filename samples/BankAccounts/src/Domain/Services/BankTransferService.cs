using BankAccounts.Domain.BankAccounts;

namespace BankAccounts.Domain.Services;

internal class BankTransferService
{
    public Result<Unit> Transfer(BankAccount fromBankAccount, BankAccount toBankAccount, decimal quantity,
        DateTime date, Guid fromMovementId, Guid toMovementId) =>
        Result
            .Create(Unit.Value)
            .Bind(_ => fromBankAccount.WithdrawMoney(fromMovementId, "Transfer", quantity, date))
            .Bind(_ => toBankAccount.MakeDeposit(toMovementId, "Transfer", quantity, date));
}
