using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A sender-created definition of a payout to a single recipient.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayoutItem : PayPalResource
{
    [JsonConverter(typeof (StringEnumConverter))]
    public PayoutRecipientType RecipientType { get; set; }

    /// <summary>The amount of money to pay the receiver.</summary>
    public PayPalCurrency Amount { get; set; }

    /// <summary>
    /// Optional. A sender-specified note for notifications. Value is any string value.
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// The receiver of the payment. Corresponds to the `recipient_type` value in the request.
    /// </summary>
    public string Receiver { get; set; }

    /// <summary>
    /// A sender-specified ID number. Tracks the batch payout in an accounting system.
    /// </summary>
    public string SenderItemId { get; set; }

    /// <summary>
    /// Obtain the status of a payout item by passing the item ID to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="payoutItemId">Payouts generated payout_item_id to obtain status.</param>
    /// <returns>PayoutItemDetails</returns>
    public static PayoutItemDetails Get(APIContext apiContext, string payoutItemId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(payoutItemId, nameof (payoutItemId));
        var resource = SdkUtil.FormatUriPath("v1/payments/payouts-item/{0}", [
            payoutItemId,
        ]);
        return ConfigureAndExecute<PayoutItemDetails>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Cancels the unclaimed payment using the items id passed in the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="payoutItemId">Payouts generated payout_item_id to obtain status.</param>
    /// <returns>PayoutItemDetails</returns>
    public static PayoutItemDetails Cancel(APIContext apiContext, string payoutItemId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(payoutItemId, nameof (payoutItemId));
        var resource = SdkUtil.FormatUriPath("v1/payments/payouts-item/{0}/cancel", [
            payoutItemId,
        ]);
        return ConfigureAndExecute<PayoutItemDetails>(apiContext, HttpMethod.Post, resource);
    }
}