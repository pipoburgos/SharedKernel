namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// The sender-provided batch header for a batch payout request.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayoutSenderBatchHeader //: PayPalSerializableObject
{
    /// <summary>
    /// A sender-specified ID number. Tracks the batch payout in an accounting system.<blockquote><strong>Note:</strong> PayPal prevents duplicate batches from being processed. If you specify a `sender_batch_id` that was used in the last 30 days, the API rejects the request and returns an error message that indicates the duplicate `sender_batch_id` and includes a HATEOAS link to the original batch payout with the same `sender_batch_id`. If you receive a HTTP `5nn` status code, you can safely retry the request with the same `sender_batch_id`. In any case, the API completes a payment only once for a specific `sender_batch_id` that is used within 30 days.</blockquote>
    /// </summary>
    public string SenderBatchId { get; set; }

    /// <summary>
    /// The subject line text for the email that PayPal sends when a payout item is completed. (The subject line is the same for all recipients.) Maximum of 255 single-byte alphanumeric characters.
    /// </summary>
    public string EmailSubject { get; set; }
    //[JsonConverter(typeof (StringEnumConverter))]
    public PayoutRecipientType RecipientType { get; set; }
}