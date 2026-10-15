using BankAccounts.Application.BankAccounts.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Queries;

namespace BankAccounts.Api.BankAccounts.Queries;

internal sealed class GetBankAccountsEndpoint : IEndpoint<BankAccountsGroup>
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapQuery("paged", Handle)
            .WithName(nameof(GetBankAccounts))
            .WithSummary("Gets bank accounts paged.");
    }

    private static async Task<Ok<IPagedList<BankAccountItem>>> Handle(IQueryBus queryBus,
        GetBankAccounts getBankAccounts, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await queryBus.Ask(getBankAccounts, cancellationToken));
    }
}