
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Line item information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceItem //: PayPalSerializableObject
{
    /// <summary>The item name.</summary>
    public string Name { get; set; }

    /// <summary>The item description.</summary>
    public string Description { get; set; }

    /// <summary>
    /// The item quantity. Valid value is from -10000 to 10000.
    /// </summary>
    public float Quantity { get; set; }

    /// <summary>
    /// The item unit price. Valid value is from -1,000,000 to 1,000,000.
    /// </summary>
    public PayPalCurrency UnitPrice { get; set; }

    /// <summary>The tax associated with the item.</summary>
    public Tax Tax { get; set; }

    /// <summary>
    /// The date when the item or service was provided. The date format is *yyyy*-*MM*-*dd* *z*, as defined in [Internet Date/Time Format](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string Date { get; set; }

    /// <summary>The item discount, as a percent or an amount value.</summary>
    public Cost Discount { get; set; }

    /// <summary>
    /// The unit of measure for the invoiced item. Value is quantity, hours, or amount.
    /// </summary>
    public string UnitOfMeasure { get; set; }
}