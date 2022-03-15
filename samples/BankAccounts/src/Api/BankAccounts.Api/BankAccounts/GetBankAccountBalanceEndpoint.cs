using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Cqrs.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Bank accounts Controller. </summary>
    [Route("api/bankAccounts", Name = "Bank Accounts")]
    public class GetBankAccountBalanceEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Gets the balance. </summary>
        [HttpGet("{bankAccountId:guid}/balance"), ResponseCache(Duration = 60)]
        public async Task<ActionResult<decimal>> Handle([FromServices] IQueryBus queryBus,
            [FromRoute] Guid bankAccountId, [FromQuery] string ownerName, CancellationToken cancellationToken)
        {
            return Ok(await queryBus.Ask(new GetBankAccountBalance(bankAccountId, ownerName), cancellationToken));
        }
    }
}
