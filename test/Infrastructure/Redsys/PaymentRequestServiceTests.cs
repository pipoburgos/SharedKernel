using Microsoft.Extensions.Options;
using NSubstitute;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.Redsys;
using SharedKernel.Infrastructure.Redsys.Services;
using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Redsys;

public class PaymentRequestServiceTests
{
    [Fact]
    public void GetPaymentRequestFormData_ShouldWork()
    {
        var optionsSnapshotMock = Substitute.For<IOptionsSnapshot<RedsysOptions>>();

        var redsysOptions = new RedsysOptions
        {
            MerchantName = "Test Merchant",
            MerchantCode = "123456",
            Currency = "978",
            TransactionType = "0",
            Terminal = "001",
            Production = false,
            Key = "Mk9m98IfEblmPfrpsawt7BmxObt98Jev",
        };

        optionsSnapshotMock.Value.Returns(redsysOptions);

        var paymentRequest = new PaymentRequest(new Uri("http://localhost/api/purchaning/made"),
            new Uri("http://sharedkernel.com/ok"), new Uri("http://sharedkernel.com/ko"), "1", 145.5m);

        IPaymentRequestService paymentRequestService = new PaymentRequestService(
            new SignatureManager(new Base64(), new TripleDes(), new Sha256()),
            new MerchantParametersManager(new Base64(), new NewtonsoftSerializer()), optionsSnapshotMock);
        var result = paymentRequestService.GetPaymentRequestFormData(paymentRequest);

        result.Ds_SignatureVersion.Should().Be(redsysOptions.SignatureVersion);
        result.Ds_MerchantParameters.Should().Be("eyJEc19NZXJjaGFudF9Db25zdW1lckxhbmd1YWdlIjoiMDAxIiwiRHNfTWVyY2hhbnRfQW1vdW50IjoiMTQ1NTAiLCJEc19NZXJjaGFudF9PcmRlciI6IjEiLCJEc19NZXJjaGFudF9NZXJjaGFudENvZGUiOiIxMjM0NTYiLCJEc19NZXJjaGFudF9DdXJyZW5jeSI6Ijk3OCIsIkRzX01lcmNoYW50X1RyYW5zYWN0aW9uVHlwZSI6IjAiLCJEc19NZXJjaGFudF9UZXJtaW5hbCI6IjAwMSIsIkRzX01lcmNoYW50X01lcmNoYW50VVJMIjoiaHR0cDovL2xvY2FsaG9zdC9hcGkvcHVyY2hhbmluZy9tYWRlIiwiRHNfTWVyY2hhbnRfVXJsT0siOiJodHRwOi8vc2hhcmVka2VybmVsLmNvbS9vayIsIkRzX01lcmNoYW50X1VybEtPIjoiaHR0cDovL3NoYXJlZGtlcm5lbC5jb20va28iLCJEc19NZXJjaGFudF9QYXlNZXRob2QiOiJDIn0=");
        result.Ds_Signature.Should().Be("3o/zSgDPG9VrNsijnIW1h44YkCRhURi9CSk95i+zZ4U=");
    }

}
