using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Queries;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Bank accounts Controller. </summary>
    [Route("api/bankAccounts", Name = "Bank Accounts")]
    public class GetBankAccountBalanceEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Gets the balance. </summary>
        [HttpGet("{bankAccountId:guid}/balance"), ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "*" })]
        public async Task<ActionResult<decimal>> Handle(Guid bankAccountId, CancellationToken cancellationToken)
        {
            return OkTyped(await QueryBus.Ask(new GetBankAccountBalance(bankAccountId), cancellationToken));
        }
    }
}
