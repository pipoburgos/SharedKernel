using PayPal.V1.Shared;
using PayPal.V1.Shared.Util;

namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Contain details of how and when the payment should be made to PayPal in cases of manual bank transfer.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentInstruction : PayPalRelationalObject
{
    /// <summary>ID of payment instruction</summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>Type of payment instruction</summary>
    public string? InstructionType { get; set; }

    /// <summary>Recipient bank Details.</summary>
    public RecipientBankingInstruction? RecipientBankingInstruction { get; set; }

    /// <summary>Amount to be transferred</summary>
    public PayPalCurrency? Amount { get; set; }

    /// <summary>Date by which payment should be received</summary>
    public string? PaymentDueDate { get; set; }

    /// <summary>Additional text regarding payment handling</summary>
    public string? Note { get; set; }

    /// <summary>
    /// Obtain the payment instruction resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="paymentId">Identifier of the Payment instruction resource to obtain the data for.</param>
    /// <returns>PaymentInstruction</returns>
    public static PaymentInstruction Get(IPayPalClient apiContext, string paymentId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(paymentId, nameof(paymentId));
        var resource = SdkUtil.FormatUriPath("v1/payments/payments/payment/{0}/payment-instruction", [
            paymentId,
        ]);
        return ConfigureAndExecute<PaymentInstruction>(apiContext, "GET", resource);
    }
}