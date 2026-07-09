namespace BankAccounts.Api.BankAccounts;

public static class BankAccountsGroup
{
    private static IEndpointRouteBuilder? _routeGroupBuilder;

    public static IEndpointRouteBuilder MapBankAccountsGroup(this IEndpointRouteBuilder app)
    {
        if (_routeGroupBuilder != null)
            return _routeGroupBuilder;

        _routeGroupBuilder = app.MapApiGroup()
            .MapGroup("bankAccounts")
            .WithDisplayName("Bank Accounts")
            .WithTags("BankAccounts");

        return _routeGroupBuilder;
    }
}
