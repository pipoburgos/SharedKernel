using BankAccounts.Api.Shared;
using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.OutputCaching;
using SharedKernel.Api;

namespace BankAccounts.Api.BankAccounts;

/// <summary> Bank accounts Controller. </summary>
[Route("api/bankAccounts", Name = "Bank Accounts")]
public sealed class GetBankAccountBalanceEndpoint : BankAccountBaseEndpoint
{
    /// <summary> Gets the balance. </summary>
    [HttpGet("{bankAccountId:guid}/balance")]
    [ResponseCache(Duration = CacheDuration.Day, VaryByQueryKeys = ["*"])]
    [OutputCache(Duration = CacheDuration.Day, VaryByQueryKeys = ["*"])]
    public async Task<ActionResult<decimal>> Handle(Guid bankAccountId, CancellationToken cancellationToken)
    {
        return OkTyped(await QueryBus.Ask(new GetBankAccountBalance(bankAccountId), cancellationToken));
    }
}