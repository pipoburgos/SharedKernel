using BankAccounts.Acceptance.Tests.Shared;
using BankAccounts.Application.BankAccounts.Commands;
using BankAccounts.Domain.BankAccounts;
using SharedKernel.Infrastructure.Requests.Middlewares.Failover;
using SharedKernel.Testing.Acceptance.Extensions;

namespace BankAccounts.Acceptance.Tests.BankAccounts
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
            var client = await _bankAccountClientFactory.CreateClientAsync();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), "Roberto", new DateTime(1890, 5, 5), "Fernández",
                Guid.NewGuid(), 250);

            var result = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            //await Task.Delay(10_000);

            _bankAccountClientFactory
                .GetNewDbContext()
                .Set<BankAccount>()
                .Any(x => x.Id == BankAccountId.Create(bankAccountId))
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task CreateBankAccountNameMoreThan100TestFailover()
        {
            var client = await _bankAccountClientFactory.CreateClientAsync();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), new string('*', 101),
                new DateTime(1980, 5, 5), "Fernández", Guid.NewGuid(), 250);

            var response = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            var ex = await response.GetErrorResponse();
            ex.Should("Name", "The length of 'Name' must be 100 characters or fewer. You entered 101 characters.");

            _bankAccountClientFactory
                .GetNewDbContext()
                .Set<ErrorRequest>()
                .Any(t => t.Request.Contains(bankAccountId.ToString().ToLower()))
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task FluentValidationSpanishOnSpanish()
        {
            var client = await _bankAccountClientFactory.CreateClientAsync("es-ES");

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), new string('*', 101),
                new DateTime(1980, 5, 5), "Fernández", Guid.NewGuid(), 250);

            var response = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            var ex = await response.GetErrorResponse();
            ex.Should("Name", "'Name' debe ser menor o igual que 100 caracteres. Ingresó 101 caracteres.");
        }

        [Fact]
        public async Task InvalidSupportedCultures()
        {
            var client = await _bankAccountClientFactory.CreateClientAsync("zh");

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), "abcde",
                new DateTime(1980, 5, 5), "Fernández", Guid.NewGuid(), 250);

            var response = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var ex = await response.GetErrorResponse();
            ex.Should("Invalud Accept-Language values.");
        }

        [Fact]
        public async Task CreateBankAccountLessThan18YearsOld()
        {
            var client = await _bankAccountClientFactory.CreateClientAsync();

            var bankAccountId = Guid.NewGuid();
            var body = new CreateBankAccount(Guid.NewGuid(), "abc",
                new DateTime(2300, 5, 5), "Fernández", Guid.NewGuid(), 250);

            var response = await client.PostAsJsonAsync($"api/bankAccounts/{bankAccountId}", body);

            var ex = await response.GetErrorResponse();
            ex.Should("Owner", "At Least 18 Years Old");
        }
    }
}
