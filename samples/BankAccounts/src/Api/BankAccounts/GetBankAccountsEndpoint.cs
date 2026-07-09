using Asp.Versioning;
using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Queries;

namespace BankAccounts.Api.BankAccounts;

internal sealed class GetBankAccountsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBankAccountsGroup()
            .MapQuery(string.Empty, Handle)
            .WithName("GetBankAccounts")
            .WithSummary("Gets bank accounts paged.")
            .Produces<IPagedList<BankAccountItem>>()
            .MapToApiVersion(new ApiVersion(1, 0))
            .WithGroupName("v1");
    }

    private static async Task<IResult> Handle(IQueryBus queryBus, [FromBody] GetBankAccounts getBankAccounts,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await queryBus.Ask(getBankAccounts, cancellationToken));
    }
}