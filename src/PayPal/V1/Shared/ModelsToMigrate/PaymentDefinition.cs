
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Defines the payment terms of a billing plan.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PaymentDefinition //: PayPalSerializableObject
{
    /// <summary>
    /// Identifier of the payment_definition. 128 characters max.
    /// </summary>
    public string Id { get; set; }

    /// <summary>Name of the payment definition. 128 characters max.</summary>
    public string Name { get; set; }

    /// <summary>
    /// Type of the payment definition. Allowed values: `TRIAL`, `REGULAR`.
    /// </summary>
    public string Type { get; set; }

    /// <summary>How frequently the customer should be charged.</summary>
    public string FrequencyInterval { get; set; }

    /// <summary>
    /// Frequency of the payment definition offered. Allowed values: `WEEK`, `DAY`, `YEAR`, `MONTH`.
    /// </summary>
    public string Frequency { get; set; }

    /// <summary>Number of cycles in this payment definition.</summary>
    public string Cycles { get; set; }

    /// <summary>
    /// Amount that will be charged at the end of each cycle for this payment definition.
    /// </summary>
    public PayPalCurrency Amount { get; set; }

    /// <summary>Array of charge_models for this payment definition.</summary>
    public List<ChargeModel> ChargeModels { get; set; }
}