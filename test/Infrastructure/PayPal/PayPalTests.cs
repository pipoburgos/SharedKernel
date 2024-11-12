using Microsoft.Extensions.DependencyInjection;
using PayPal.V1;
using PayPal.V1.Payments;
using PayPal.V1.Payments.Payments;
using PayPal.V1.Shared;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.PayPal;
using SharedKernel.Testing.Infrastructure;
using System.Net.Http.Headers;
using System.Text;

namespace SharedKernel.Integration.Tests.PayPal;
public class PayPalTests : InfrastructureTestCase<FakeStartup>
{
    private const string ClientId = "AUv8rrc_P-EbP2E0mpb49BV7rFt3Usr-vdUZO8VGOnjRehGHBXkSzchr37SYF2GNdQFYSp72jh5QUhzG";
    private const string ClientSecret = "EMnAWe06ioGtouJs7gLYT9chK9-2jJ--7MKRXpI8FesmY_2Kp-d_7aCqff7M9moEJBvuXoBO4clKtY0v";

    protected override string GetJsonFile()
    {
        return "PayPal/appsettings.PayPal.json";
    }


    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services.AddPayPal(Configuration, options =>
        {
            options.Settings.ClientId = ClientId;
            options.Settings.ClientSecret = ClientSecret;
        }).AddNewtonsoftSerializer();
    }

    [Fact]
    public void Test()
    {
        var client = GetRequiredServiceOnNewScope<IPayPalClient>();

        const string currency = "EUR";
        var price = 125.75.ToString("N2").Replace(",", ".");

        var invoiceNumber = Guid.NewGuid().ToString();

        var itemList = new ItemList();
        itemList.Items.Add(new Item
        {
            Name = "Windows License",
            Currency = currency,
            Price = price,
            Quantity = 1.ToString(),
            Sku = invoiceNumber,
        });

        var transactionList = new List<Transaction>
        {
            new Transaction
            {
                Description = "Windows License",
                InvoiceNumber = invoiceNumber,
                Amount = new Amount
                {
                    Currency = currency,
                    Total = price,
                },
                ItemList = itemList,
            },
        };
        // Adding description about the transaction

        var payPalPaymentP = new Payment
        {
            Intent = "sale",
            Payer = new Payer
            {
                PaymentMethod = "paypal",
            },
            Transactions = transactionList,
            RedirectUrls = new RedirectUrls
            {
                CancelUrl = "https://google.es/ko",
                ReturnUrl = "https://google.es/ok",
            },
        };

        // Create a payment using a APIContext
        var createdPayment = payPalPaymentP.Create(client);

        createdPayment.Id.Should().NotBeNullOrWhiteSpace();

        var response = createdPayment.Links?
            .SingleOrDefault(lnk => lnk.Rel != null && lnk.Rel.ToLower().Trim().Equals("approval_url"));

        response.Should().NotBeNull();
        response!.Href.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetAccessTokenAsync()
    {
        using var httpClient = new HttpClient();

        var jsonSerializer = GetRequiredServiceOnNewScope<IJsonSerializer>();
        // Configura la autenticación básica con ClientId y SecretId en Base64
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        // Configura el contenido de la solicitud en application/x-www-form-urlencoded
        var requestData = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        // Realiza la solicitud POST
        var response = await httpClient.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", requestData);

        // Verifica si la solicitud fue exitosa
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }

        // Deserializa la respuesta en la clase PayPalTokenResponse
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var token = jsonSerializer.Deserialize<PayPalTokenResponse>(jsonResponse, NamingConvention.SnakeCase);

        token.Should().NotBeNull();
        token.ExpiresIn.Should().BeGreaterThan(0);
        token.AccessToken.Should().NotBeNullOrWhiteSpace();
        token.AppId.Should().NotBeNullOrWhiteSpace();
        token.Nonce.Should().NotBeNullOrWhiteSpace();
        token.Scope.Should().NotBeNullOrWhiteSpace();
        token.TokenType.Should().NotBeNullOrWhiteSpace();
    }
}
