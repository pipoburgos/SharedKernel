using SharedKernel.Api.Endpoints;

namespace BankAccounts.Api.BankAccounts;

public class BankAccountsGroup : IEndpointGroup
{
    public string Name => "BankAccounts";
    public string Prefix => "/api/v{version:apiVersion}/bank-accounts";

    public void Configure(RouteGroupBuilder group)
    {
        group
            .WithTags(Name)
            .WithDisplayName("Bank Accounts")
            .HasApiVersion(1)
            .HasApiVersion(2);
    }
}
