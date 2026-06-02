using BankAccounts.Application.BankAccounts.Queries;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Queries;

namespace BankAccounts.Api.BankAccounts;

internal sealed class GetBankAccountsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBankAccountsGroup()
            .MapPost(string.Empty, Handle)
            .WithName("GetBankAccounts")
            .WithSummary("Gets bank accounts paged.")
            .Produces<IPagedList<BankAccountItem>>();
    }

    private static async Task<IResult> Handle(IQueryBus queryBus, GetBankAccounts getBankAccounts, CancellationToken cancellationToken)
    {
        return Results.Ok(await queryBus.Ask(getBankAccounts, cancellationToken));
    }
}