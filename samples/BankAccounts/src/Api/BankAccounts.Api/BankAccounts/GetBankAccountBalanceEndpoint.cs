using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Cqrs.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Create a bank account. </summary>
    [Route("api/bankAccounts", Name = "Bank Accounts Controller")]
    public class GetBankAccountBalanceEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Gets the balance. </summary>
        /// <param name="queryBus"></param>
        /// <param name="id"></param>
        /// <param name="ownerName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}/balance"), ResponseCache(Duration = 60)]
        public async Task<ActionResult<decimal>> Handle([FromServices] IQueryBus queryBus,
            [FromRoute] Guid id, [FromQuery] string ownerName, CancellationToken cancellationToken)
        {
            return Ok(await queryBus.Ask(new GetBankAccountBalance(id, ownerName), cancellationToken));
        }
    }
}
