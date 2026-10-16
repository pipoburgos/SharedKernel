using BankAccounts.Acceptance.Tests.Shared;
using BankAccounts.Application.BankAccounts.Commands;
using BankAccounts.Application.BankAccounts.Queries;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Testing.Acceptance.Extensions;

namespace BankAccounts.Acceptance.Tests.BankAccounts.Queries;

[Collection("Factory")]
public class GetBankAccountsEndpointTests
{
    private readonly BankAccountClientFactory _factory;

    public GetBankAccountsEndpointTests(BankAccountClientFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetSessionsManualsEndpointReturnsSuccessResult()
    {

        using var client = await _factory.CreateClientAsync();

        var bankAccountId = Guid.NewGuid();
        var body = new CreateBankAccount(Guid.NewGuid(), "Roberto", new DateTime(1890, 5, 5), "Fernández", Guid.NewGuid(), 250);

        var result = await client.PostAsJsonAsync($"api/v1/bank-accounts/{bankAccountId}", body, TestContext.Current.CancellationToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        using var response = await client.PostAsJsonAsync("api/v1/bank-accounts/paged", new GetBankAccounts(), TestContext.Current.CancellationToken);

        var pagedList = await response.GetResponseContentAsync<PagedList<BankAccountItem>>();
        pagedList.TotalRecords.Should().BeGreaterThan(0);
    }


    [Fact]
    public async Task GetSessionsManualsEndpointReturnsColumnNotFoundResult()
    {
        using var client = await _factory.CreateClientAsync();

        var bankAccountId = Guid.NewGuid();
        var body = new CreateBankAccount(Guid.NewGuid(), "Roberto", new DateTime(1890, 5, 5), "Fernández", Guid.NewGuid(), 250);

        var result = await client.PostAsJsonAsync($"api/v1/bank-accounts/{bankAccountId}", body, TestContext.Current.CancellationToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        using var response = await client.PostAsJsonAsync("api/v1/bank-accounts/paged", new GetBankAccounts(new PageOptions(orders: [new("not_found")])), TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        error.Should().Be("\"An error has occurred, check with the administrator (Order Property Not_found not found)\"");
    }
}