using BankAccounts.Application.BankAccounts.Queries;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Queries;

namespace BankAccounts.Api.BankAccounts.Queries;

internal sealed class GetBankAccountBalanceEndpoint : IEndpoint<BankAccountsGroup>
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("{bankAccountId:guid}/balance", Handle)
            .WithName("GetBankAccountBalance")
            .WithSummary("Gets the balance of a bank account.")
            .Produces<decimal>();
        //.WithMetadata(new ResponseCacheAttribute
        //{
        //    Duration = CacheDuration.Day,
        //    VaryByQueryKeys = ["*"],
        //})
        //.CacheOutput(policy =>
        //{
        //    policy.Expire(TimeSpan.FromDays(1));
        //    policy.SetVaryByQuery("*");
        //});
    }

    private static async Task<IResult> Handle(IQueryBus queryBus, Guid bankAccountId,
        CancellationToken cancellationToken)
    {
        var result = await queryBus.Ask(new GetBankAccountBalance(bankAccountId), cancellationToken);
        return Results.Ok(result);
    }
}