namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>Address used in context of Log In with PayPal.</summary>
public class Address
{
    /// <summary>
    /// Street address component, which may include house number, and street name
    /// </summary>
    public string StreetAddress { get; set; }

    /// <summary>City or locality component</summary>
    public string Locality { get; set; }

    /// <summary>State, province, prefecture or region component</summary>
    public string Region { get; set; }

    /// <summary>Zip code or postal code component</summary>
    public string PostalCode { get; set; }

    /// <summary>Country name component.</summary>
    public string Country { get; set; }
}