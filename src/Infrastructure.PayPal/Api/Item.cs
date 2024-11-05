using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Item details.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Item //: PayPalSerializableObject
{
    /// <summary>Stock keeping unit corresponding (SKU) to item.</summary>
    public string Sku { get; set; }

    /// <summary>Item name. 127 characters max.</summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the item. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string Description { get; set; }

    /// <summary>Number of a particular item. 10 characters max.</summary>
    public string Quantity { get; set; }

    /// <summary>Item cost. 10 characters max.</summary>
    public string Price { get; set; }

    /// <summary>
    /// 3-letter [currency code](https://developer.paypal.com/docs/integration/direct/rest_api_payment_country_currency_support/).
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Tax of the item. Only supported when the `payment_method` is set to `paypal`.
    /// </summary>
    public string Tax { get; set; }

    /// <summary>
    /// URL linking to item information. Available to payer in transaction history.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Set of optional data used for PayPal risk determination.
    /// </summary>
    public List<NameValuePair> SupplementaryData { get; set; }

    /// <summary>
    /// Set of optional data used for PayPal post-transaction notifications.
    /// </summary>
    public List<NameValuePair> PostbackData { get; set; }

    /// <summary>Category type of the item.</summary>
    [JsonIgnore]
    [Obsolete("This field is not availalbe publicly in the PayPal REST API. It will be removed in a future release.", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string Category { get; set; }

    /// <summary>Weight of the item.</summary>
    [JsonIgnore]
    [Obsolete("This field is not availalbe publicly in the PayPal REST API. It will be removed in a future release.", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public Measurement Weight { get; set; }

    /// <summary>Length of the item.</summary>
    [JsonIgnore]
    [Obsolete("This field is not availalbe publicly in the PayPal REST API. It will be removed in a future release.", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public Measurement Length { get; set; }

    /// <summary>Height of the item.</summary>
    [JsonIgnore]
    [Obsolete("This field is not availalbe publicly in the PayPal REST API. It will be removed in a future release.", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public Measurement Height { get; set; }

    /// <summary>Width of the item.</summary>
    [JsonIgnore]
    [Obsolete("This field is not availalbe publicly in the PayPal REST API. It will be removed in a future release.", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public Measurement Width { get; set; }
}