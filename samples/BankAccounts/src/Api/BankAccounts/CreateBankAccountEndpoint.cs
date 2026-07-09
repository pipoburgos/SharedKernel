using Asp.Versioning;
using BankAccounts.Application.BankAccounts.Commands;
using SharedKernel.Api.Endpoints;
using SharedKernel.Application.Cqrs.Commands;

namespace BankAccounts.Api.BankAccounts;

internal sealed class CreateBankAccountEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBankAccountsGroup()
            .MapPost("{bankAccountId:guid}", Handle)
            .WithName("CreateBankAccount")
            .WithSummary("Create a bank account.")
            .MapToApiVersion(new ApiVersion(1, 0))
            .WithGroupName("v1");
    }

    private static async Task<IResult> Handle(ICommandBus commandBus, Guid bankAccountId,
        CreateBankAccount createBankAccount, CancellationToken cancellationToken)
    {
        createBankAccount.AddId(bankAccountId);
        var result = await commandBus.Dispatch(createBankAccount, cancellationToken);
        return result.ToIResult();
    }
}