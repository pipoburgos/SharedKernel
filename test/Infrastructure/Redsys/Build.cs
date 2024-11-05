using SharedKernel.Infrastructure.Redsys;

namespace SharedKernel.Integration.Tests.Redsys;

public static class Build
{
    public static PaymentResponse PaymentResponse(bool paid)
    {
        return new PaymentResponse
        {
            Ds_Amount = "123",
            Ds_AuthorisationCode = "0",
            Ds_Card_Country = "1",
            Ds_Card_Type = "D",
            Ds_ConsumerLanguage = "",
            Ds_Currency = "978",
            Ds_Date = "01/01/2015",
            Ds_Hour = "22:00",
            Ds_MerchantCode = "TEST",
            Ds_MerchantData = "",
            Ds_Order = "",
            Ds_Response = paid ? "0000" : "101",
            Ds_SecurePayment = "",
            Ds_Terminal = "1",
            Ds_TransactionType = "0",
        };
    }
}