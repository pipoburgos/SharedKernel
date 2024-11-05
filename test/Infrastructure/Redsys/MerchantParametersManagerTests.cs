using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.Redsys.Contracts;
using SharedKernel.Infrastructure.Redsys.Services;
using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Integration.Tests.Redsys;

public class MerchantParametersManagerTests
{
    //[TestMethod]
    //public void GetMerchantParameters_ShouldWork()
    //{
    //    var paymentRequest = new PaymentRequest("", "999008881", "871", "0", "145", "978", "19990000000A", "", "");
    //    IMerchantParametersManager merchantParamentersManager = new MerchantParametersManager();
    //    var result = merchantParamentersManager.GetMerchantParameters(paymentRequest);

    //    Assert.Equals(result, "eyJEc19NZXJjaGFudF9Db25zdW1lckxhbmd1YWdlIjoiMDAxIiwiRHNfTWVyY2hhbnRfQW1vdW50IjoiMCIsIkRzX01lcmNoYW50X09yZGVyIjoiOTc4IiwiRHNfTWVyY2hhbnRfTWVyY2hhbnRDb2RlIjoiIiwiRHNfTWVyY2hhbnRfQ3VycmVuY3kiOiIxNDUiLCJEc19NZXJjaGFudF9UcmFuc2FjdGlvblR5cGUiOiI4NzEiLCJEc19NZXJjaGFudF9UZXJtaW5hbCI6Ijk5OTAwODg4MSIsIkRzX01lcmNoYW50X01lcmNoYW50VVJMIjoiMTk5OTAwMDAwMDBBIiwiRHNfTWVyY2hhbnRfVXJsT0siOiIiLCJEc19NZXJjaGFudF9VcmxLTyI6IiIsIkRzX01lcmNoYW50X1BheU1ldGhvZCI6IkMifQ==");
    //}

    //[TestMethod]
    //public void GetMerchantParameters_ShouldWork_2()
    //{
    //    var paymentRequest = new PaymentRequest("", "999008881", "871", "0", "145", "978", "999911111111", "", "");
    //    IMerchantParametersManager merchantParamentersManager = new MerchantParametersManager();
    //    var result = merchantParamentersManager.GetMerchantParameters(paymentRequest);

    //    Assert.Equals(result, "eyJEc19NZXJjaGFudF9Db25zdW1lckxhbmd1YWdlIjoiMDAxIiwiRHNfTWVyY2hhbnRfQW1vdW50IjoiMCIsIkRzX01lcmNoYW50X09yZGVyIjoiOTc4IiwiRHNfTWVyY2hhbnRfTWVyY2hhbnRDb2RlIjoiIiwiRHNfTWVyY2hhbnRfQ3VycmVuY3kiOiIxNDUiLCJEc19NZXJjaGFudF9UcmFuc2FjdGlvblR5cGUiOiI4NzEiLCJEc19NZXJjaGFudF9UZXJtaW5hbCI6Ijk5OTAwODg4MSIsIkRzX01lcmNoYW50X01lcmNoYW50VVJMIjoiOTk5OTExMTExMTExIiwiRHNfTWVyY2hhbnRfVXJsT0siOiIiLCJEc19NZXJjaGFudF9VcmxLTyI6IiIsIkRzX01lcmNoYW50X1BheU1ldGhvZCI6IkMifQ==");
    //}

    [Fact]
    public void GetPaymentResponse_ShouldWork()
    {
        const string merchantParamenters = "eyJEc19EYXRlIjoiMTkvMDgvMjAxNSIsIkRzX0hvdXIiOiIxMjo0OSIsIkRzX0Ftb3VudCI6IjEyMzQ1IiwiRHNfQ3VycmVuY3kiOiI5NzgiLCJEc19PcmRlciI6Ijk5OTkxMTExMjIyMiIsIkRzX01lcmNoYW50Q29kZSI6IjAxMjM0NTYiLCJEc19UZXJtaW5hbCI6IjIiLCJEc19SZXNwb25zZSI6IjAiLCJEc19NZXJjaGFudERhdGEiOiIiLCJEc19TZWN1cmVQYXltZW50IjoiMSIsIkRzX1RyYW5zYWN0aW9uVHlwZSI6IjAiLCJEc19DYXJkX0NvdW50cnkiOiIiLCJEc19BdXRob3Jpc2F0aW9uQ29kZSI6IjAiLCJEc19Db25zdW1lckxhbmd1YWdlIjoiMCIsIkRzX0NhcmRfVHlwZSI6IkQifQ==";
        IMerchantParametersManager merchantParamentersManager = new MerchantParametersManager(new Base64(), new NewtonsoftSerializer());
        var paymentResponse = merchantParamentersManager.GetPaymentResponse(merchantParamenters);

        paymentResponse.Should().NotBeNull();
        paymentResponse.Ds_Amount.Should().Be("12345");
        paymentResponse.Ds_AuthorisationCode.Should().Be("0");
        paymentResponse.Ds_Card_Country.Should().Be("");
        paymentResponse.Ds_Card_Type.Should().Be("D");
        paymentResponse.Ds_ConsumerLanguage.Should().Be("0");
        paymentResponse.Ds_Currency.Should().Be("978");
        paymentResponse.Ds_Date.Should().Be("19/08/2015");
        paymentResponse.Ds_Hour.Should().Be("12:49");
        paymentResponse.Ds_MerchantCode.Should().Be("0123456");
        paymentResponse.Ds_MerchantData.Should().Be("");
        paymentResponse.Ds_Order.Should().Be("999911112222");
        paymentResponse.Ds_Response.Should().Be("0");
        paymentResponse.Ds_SecurePayment.Should().Be("1");
        paymentResponse.Ds_Terminal.Should().Be("2");
        paymentResponse.Ds_TransactionType.Should().Be("0");
    }
}