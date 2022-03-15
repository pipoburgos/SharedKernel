using BankAccounts.Application.BankAccounts.Commands;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

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
            var client = _bankAccountClientFactory.CreateClient();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), "Roberto", new DateTime(1890, 5, 5), "Fernández",
                Guid.NewGuid(), 250);

            var result = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task CreateBankAccountNameMoreThan100()
        {
            var client = _bankAccountClientFactory.CreateClient();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), "Robertooooooooooooooooooooooooooooooooooooooooooooooooooooooo",
                new DateTime(2022, 5, 5), "Fernández", Guid.NewGuid(), 250);

            var result = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);


            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var text = await result.Content.ReadAsStringAsync();
            text.Should().Contain("18");
        }
    }
}
