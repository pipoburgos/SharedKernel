
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Participant information.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Participant //: PayPalSerializableObject
{
    /// <summary>The participant email address.</summary>
    public string Email { get; set; }

    /// <summary>The participant first name.</summary>
    public string FirstName { get; set; }

    /// <summary>The participant last name.</summary>
    public string LastName { get; set; }

    /// <summary>The participant company business name.</summary>
    public string BusinessName { get; set; }

    /// <summary>The participant phone number.</summary>
    public Phone Phone { get; set; }

    /// <summary>The participant fax number.</summary>
    public Phone Fax { get; set; }

    /// <summary>The participant website.</summary>
    public string Website { get; set; }

    /// <summary>Additional information, such as business hours.</summary>
    public string AdditionalInfo { get; set; }

    /// <summary>The participant address.</summary>
    public Address Address { get; set; }
}