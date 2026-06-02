namespace BankAccounts.Api.BankAccounts;

public static class BankAccountsGroup
{
    public static IEndpointRouteBuilder MapBankAccountsGroup(this IEndpointRouteBuilder app)
    {
        return app
            .MapGroup("api/bankAccounts")
            .WithDisplayName("Bank Accounts")
            .WithTags("BankAccounts");
    }
}
