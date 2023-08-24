using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Commands;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Bank accounts Controller. </summary>
    [Route("api/bankAccounts", Name = "Bank Accounts")]
    public class CreateBankAccountEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Create a bank account. </summary>
        [HttpPost("{bankAccountId:guid}")]
        public async Task<IActionResult> Handle(//[FromServices] ICommandBusAsync commandBusAsync,
            Guid bankAccountId, CreateBankAccount createBankAccount, CancellationToken cancellationToken)
        {
            createBankAccount?.AddId(bankAccountId);
            return OkTyped(await CommandBus.Dispatch(createBankAccount, cancellationToken));
        }
    }
}
