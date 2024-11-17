using Microsoft.Extensions.Options;
using NSubstitute;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.Redsys;
using SharedKernel.Infrastructure.Redsys.Services;
using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Redsys;

public class PaymentResponseServiceTests
{
    private readonly IPaymentResponseService _paymentResponseService;

    public PaymentResponseServiceTests()
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

        _paymentResponseService = new PaymentResponseService(
            new MerchantParametersManager(new Base64(), new NewtonsoftSerializer()),
            new SignatureManager(new Base64(), new TripleDes(), new Sha256()), new SignatureComparer(),
            optionsSnapshotMock);
    }

    [Fact]
    public void GetProcessedPayment_ShouldWork_WhenSignatureIsValidAndPayIsPerformed()
    {
        const string merchantParamenters = "eyJEc19EYXRlIjoiMTkvMDgvMjAxNSIsIkRzX0hvdXIiOiIxMjo0OSIsIkRzX0Ftb3VudCI6IjEyMzQ1IiwiRHNfQ3VycmVuY3kiOiI5NzgiLCJEc19PcmRlciI6Ijk5OTkxMTExMjIyMiIsIkRzX01lcmNoYW50Q29kZSI6IjAxMjM0NTYiLCJEc19UZXJtaW5hbCI6IjIiLCJEc19SZXNwb25zZSI6IjAiLCJEc19NZXJjaGFudERhdGEiOiIiLCJEc19TZWN1cmVQYXltZW50IjoiMSIsIkRzX1RyYW5zYWN0aW9uVHlwZSI6IjAiLCJEc19DYXJkX0NvdW50cnkiOiIiLCJEc19BdXRob3Jpc2F0aW9uQ29kZSI6IjAiLCJEc19Db25zdW1lckxhbmd1YWdlIjoiMCIsIkRzX0NhcmRfVHlwZSI6IkQifQ==";
        const string platformSignature = "UpSJaiLH6mMZZkXQAyMgImD4LaJZLInbHaN7zzKbYr4=";


        var result = _paymentResponseService.GetProcessedPayment(merchantParamenters, platformSignature);

        result.IsValidSignature.Should().BeTrue();
        result.IsPaymentPerformed.Should().BeTrue();
        result.PaymentResponse.Should().NotBeNull();
        result.PaymentResponse.Ds_Amount.Should().Be("12345");
        result.PaymentResponse.Ds_AuthorisationCode.Should().Be("0");
        result.PaymentResponse.Ds_Card_Country.Should().Be("");
        result.PaymentResponse.Ds_Card_Type.Should().Be("D");
        result.PaymentResponse.Ds_ConsumerLanguage.Should().Be("0");
        result.PaymentResponse.Ds_Currency.Should().Be("978");
        result.PaymentResponse.Ds_Date.Should().Be("19/08/2015");
        result.PaymentResponse.Ds_Hour.Should().Be("12:49");
        result.PaymentResponse.Ds_MerchantCode.Should().Be("0123456");
        result.PaymentResponse.Ds_MerchantData.Should().Be("");
        result.PaymentResponse.Ds_Order.Should().Be("999911112222");
        result.PaymentResponse.Ds_Response.Should().Be("0");
        result.PaymentResponse.Ds_SecurePayment.Should().Be("1");
        result.PaymentResponse.Ds_Terminal.Should().Be("2");
        result.PaymentResponse.Ds_TransactionType.Should().Be("0");
    }

    [Fact]
    public void GetProcessedPayment_ShouldWork_WhenSignatureIsNotValid()
    {
        const string merchantParamenters = "eyJEc19EYXRlIjoiMTkvMDgvMjAxNSIsIkRzX0hvdXIiOiIxMjo0OSIsIkRzX0Ftb3VudCI6IjEyMzQ1IiwiRHNfQ3VycmVuY3kiOiI5NzgiLCJEc19PcmRlciI6Ijk5OTkxMTExMjIyMiIsIkRzX01lcmNoYW50Q29kZSI6IjAxMjM0NTYiLCJEc19UZXJtaW5hbCI6IjIiLCJEc19SZXNwb25zZSI6IjAiLCJEc19NZXJjaGFudERhdGEiOiIiLCJEc19TZWN1cmVQYXltZW50IjoiMSIsIkRzX1RyYW5zYWN0aW9uVHlwZSI6IjAiLCJEc19DYXJkX0NvdW50cnkiOiIiLCJEc19BdXRob3Jpc2F0aW9uQ29kZSI6IjAiLCJEc19Db25zdW1lckxhbmd1YWdlIjoiMCIsIkRzX0NhcmRfVHlwZSI6IkQifQ==";
        const string platformSignature = "xxxxxx";

        var result = _paymentResponseService.GetProcessedPayment(merchantParamenters, platformSignature);

        result.IsValidSignature.Should().BeFalse();
        result.IsPaymentPerformed.Should().BeFalse();
    }

    [Fact]
    public void GetProcessedPayment_ShouldWork_WhenSignatureIsValidAndPayIsNotPerformed()
    {
        const string merchantParamenters = "eyJEc19EYXRlIjoiMTkvMDgvMjAxNSIsIkRzX0hvdXIiOiIxMjo0OSIsIkRzX0Ftb3VudCI6IjEyMzQ1IiwiRHNfQ3VycmVuY3kiOiI5NzgiLCJEc19PcmRlciI6Ijk5OTkxMTExMjIyMiIsIkRzX01lcmNoYW50Q29kZSI6IjAxMjM0NTYiLCJEc19UZXJtaW5hbCI6IjIiLCJEc19SZXNwb25zZSI6IjEwMSIsIkRzX01lcmNoYW50RGF0YSI6IiIsIkRzX1NlY3VyZVBheW1lbnQiOiIxIiwiRHNfVHJhbnNhY3Rpb25UeXBlIjoiMCIsIkRzX0NhcmRfQ291bnRyeSI6IiIsIkRzX0F1dGhvcmlzYXRpb25Db2RlIjoiMCIsIkRzX0NvbnN1bWVyTGFuZ3VhZ2UiOiIwIiwiRHNfQ2FyZF9UeXBlIjoiRCJ9";
        const string platformSignature = "iRNNn9pg2j6LIaLtlm15998hp/li7e2ptwVyfmO5JAY=";

        var result = _paymentResponseService.GetProcessedPayment(merchantParamenters, platformSignature);

        result.IsValidSignature.Should().BeTrue();
        result.IsPaymentPerformed.Should().BeFalse();
        result.PaymentResponse.Should().NotBeNull();
        result.PaymentResponse.Ds_Amount.Should().Be("12345");
        result.PaymentResponse.Ds_AuthorisationCode.Should().Be("0");
        result.PaymentResponse.Ds_Card_Country.Should().Be("");
        result.PaymentResponse.Ds_Card_Type.Should().Be("D");
        result.PaymentResponse.Ds_ConsumerLanguage.Should().Be("0");
        result.PaymentResponse.Ds_Currency.Should().Be("978");
        result.PaymentResponse.Ds_Date.Should().Be("19/08/2015");
        result.PaymentResponse.Ds_Hour.Should().Be("12:49");
        result.PaymentResponse.Ds_MerchantCode.Should().Be("0123456");
        result.PaymentResponse.Ds_MerchantData.Should().Be("");
        result.PaymentResponse.Ds_Order.Should().Be("999911112222");
        result.PaymentResponse.Ds_Response.Should().Be("101");
        result.PaymentResponse.Ds_SecurePayment.Should().Be("1");
        result.PaymentResponse.Ds_Terminal.Should().Be("2");
        result.PaymentResponse.Ds_TransactionType.Should().Be("0");
    }
}