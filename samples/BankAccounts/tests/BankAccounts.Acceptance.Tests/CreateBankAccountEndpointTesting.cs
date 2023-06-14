using BankAccounts.Acceptance.Tests.Shared;
using BankAccounts.Application.BankAccounts.Commands;
using BankAccounts.Infrastructure.Shared.Data;
using SharedKernel.Testing.Acceptance.Extensions;

namespace BankAccounts.Acceptance.Tests
{
    [Collection("Factory")]
    public class CreateBankAccountEndpointTesting
    {
        private readonly BankAccountClientFactory _bankAccountClientFactory;

        public CreateBankAccountEndpointTesting(BankAccountClientFactory bankAccountClientFactory)
        {
            _bankAccountClientFactory = bankAccountClientFactory;
        }

        [Fact]
        public async Task CreateBankAccountOk()
        {
            var client = await _bankAccountClientFactory.CreateClientAsync<BankAccountDbContext>();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), "Roberto", new DateTime(1890, 5, 5), "Fernández",
                Guid.NewGuid(), 250);

            var result = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task CreateBankAccountNameMoreThan100()
        {
            var client = await _bankAccountClientFactory.CreateClientAsync<BankAccountDbContext>();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), new string('*', 101),
                new DateTime(1980, 5, 5), "Fernández", Guid.NewGuid(), 250);

            var response = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            var ex = await response.GetErrorResponse();
            ex.Should("Name", "The length of 'Name' must be 100 characters or fewer. You entered 101 characters.");
        }
    }
}
