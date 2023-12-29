using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Commands;

namespace BankAccounts.Api.BankAccounts;

/// <summary> Bank accounts Controller. </summary>
[Route("api/bankAccounts", Name = "Bank Accounts")]
public sealed class CreateBankAccountEndpoint : BankAccountBaseEndpoint
{
    /// <summary> Create a bank account. </summary>
    [HttpPost("{bankAccountId:guid}")]
    public Task<IActionResult> Handle(Guid bankAccountId, CreateBankAccount createBankAccount, CancellationToken cancellationToken)
    {
        createBankAccount.AddId(bankAccountId);
        return OkTyped(CommandBus.Dispatch(createBankAccount, cancellationToken));
    }
}