using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Commands;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Cqrs.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccounts.Api.BankAccounts
{
    /// <summary> Create a bank account. </summary>
    [Route("api/bankAccounts", Name = "Bank Acounts Controller")]
    public class CreateBankAccountEndpoint : BankAccountBaseEndpoint
    {
        /// <summary> Create a bank account. </summary>
        /// <param name="commandBus"></param>
        /// <param name="bankAccountId"></param>
        /// <param name="createBankAccount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
