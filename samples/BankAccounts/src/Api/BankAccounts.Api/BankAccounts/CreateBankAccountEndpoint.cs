using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Commands;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Cqrs.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Bank accounts Controller. </summary>
    [Route("api/bankAccounts", Name = "Bank Accounts")]
    public class CreateBankAccountEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Create a bank account. </summary>
        [HttpPost("{bankAccountId:guid}")]
        public async Task<IActionResult> Handle([FromServices] ICommandBus commandBus, [FromRoute] Guid bankAccountId,
            [FromBody] CreateBankAccount createBankAccount, CancellationToken cancellationToken)
        {
            createBankAccount?.AddId(bankAccountId);
            await commandBus.Dispatch(createBankAccount, cancellationToken);
            return Ok();
        }
    }
}
