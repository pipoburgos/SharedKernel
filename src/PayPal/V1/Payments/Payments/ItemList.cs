
namespace PayPal.V1.Payments.Payments;

/// <summary>
/// Details of a list of purchasable items and shipping information included with a payment transaction.
/// </summary>
public class ItemList //: PayPalSerializableObject
{
    /// <summary>
    /// 
    /// </summary>
    public ItemList()
    {
        Items = new List<Item>();
    }

    /// <summary>List of items.</summary>
    public List<Item> Items { get; set; }

    /// <summary>Shipping address.</summary>
    public ShippingAddress? ShippingAddress { get; set; }

    /// <summary>
    /// Shipping method used for this payment like USPSParcel etc.
    /// </summary>
    public string? ShippingMethod { get; set; }

    /// <summary>
    /// Allows merchant's to share payer’s contact number with PayPal for the current payment. Final contact number of payer associated with the transaction might be same as shipping_phone_number or different based on Payer’s action on PayPal. The phone number must be represented in its canonical international format, as defined by the E.164 numbering plan
    /// </summary>
    public string? ShippingPhoneNumber { get; set; }
}