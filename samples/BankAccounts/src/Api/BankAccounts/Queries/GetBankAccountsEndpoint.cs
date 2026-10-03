using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Queries;

namespace BankAccounts.Api.BankAccounts.Queries;

internal sealed class GetBankAccountsEndpoint : IEndpoint<BankAccountsGroup>
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapQuery(string.Empty, Handle)
            .WithName("GetBankAccounts")
            .WithSummary("Gets bank accounts paged.")
            .Produces<IPagedList<BankAccountItem>>();
    }

    private static async Task<IResult> Handle(IQueryBus queryBus, [FromBody] GetBankAccounts getBankAccounts,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await queryBus.Ask(getBankAccounts, cancellationToken));
    }
}