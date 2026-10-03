using BankAccounts.Application.BankAccounts.Commands;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Commands;

namespace BankAccounts.Api.BankAccounts.Commands;

internal sealed class CreateBankAccountEndpoint : IEndpoint<BankAccountsGroup>
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("{bankAccountId:guid}", Handle)
            .WithName("CreateBankAccount")
            .WithSummary("Create a bank account.");
    }

    private static async Task<IResult> Handle(ICommandBus commandBus, Guid bankAccountId,
        CreateBankAccount createBankAccount, CancellationToken cancellationToken)
    {
        createBankAccount.AddId(bankAccountId);
        var result = await commandBus.Dispatch(createBankAccount, cancellationToken);
        return result.ToIResult();
    }
}