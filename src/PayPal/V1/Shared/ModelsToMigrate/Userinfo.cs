using PayPal.V1.Shared.Util;

namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>User information in context of Log In with PayPal.</summary>
public class Userinfo : PayPalResource
{
    /// <summary>Subject - Identifier for the End-User at the Issuer</summary>
    public string UserId { get; set; }

    /// <summary>Subject - Identifier for the End-User at the Issuer</summary>
    public string Sub { get; set; }

    /// <summary>
    /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences
    /// </summary>
    public string Name { get; set; }

    /// <summary>Given name(s) or first name(s) of the End-User</summary>
    public string GivenName { get; set; }

    /// <summary>Surname(s) or last name(s) of the End-User</summary>
    public string FamilyName { get; set; }

    /// <summary>Middle name(s) of the End-User</summary>
    public string MiddleName { get; set; }

    /// <summary>URL of the End-User's profile picture</summary>
    public string Picture { get; set; }

    /// <summary>End-User's preferred e-mail address</summary>
    public string Email { get; set; }

    /// <summary>
    /// True if the End-User's e-mail address has been verified; otherwise false
    /// </summary>
    public bool? EmailVerified { get; set; }

    /// <summary>End-User's gender.</summary>
    public string Gender { get; set; }

    /// <summary>
    /// End-User's birthday, represented as an YYYY-MM-DD format. They year MAY be 0000, indicating it is omited. To represent only the year, YYYY format would be used
    /// </summary>
    public string Birthdate { get; set; }

    /// <summary>
    /// Time zone database representing the End-User's time zone
    /// </summary>
    public string Zoneinfo { get; set; }

    /// <summary>End-User's locale.</summary>
    public string Locale { get; set; }

    /// <summary>End-User's preferred telephone number</summary>
    public string PhoneNumber { get; set; }

    /// <summary>End-User's preferred address</summary>
    public Address Address { get; set; }

    /// <summary>Verified account status</summary>
    public bool? VerifiedAccount { get; set; }

    /// <summary>Account type.</summary>
    public string AccountType { get; set; }

    /// <summary>Account holder age range.</summary>
    public string AgeRange { get; set; }

    /// <summary>Account payer identifier.</summary>
    public string PayerId { get; set; }

    /// <summary>
    /// Returns user details
    /// <param name="userinfoParameters">Query parameters used for API call</param>
    /// </summary>
    public static Userinfo GetUserinfo(UserinfoParameters userinfoParameters)
    {
        return GetUserinfo(null, userinfoParameters);
    }

    /// <summary>
    /// Returns user details
    /// <param name="apiContext">IPayPalClient to be used for the call.</param>
    /// <param name="userinfoParameters">Query parameters used for API call</param>
    /// </summary>
    public static Userinfo GetUserinfo(IPayPalClient apiContext, UserinfoParameters userinfoParameters)
    {
        var resource = SdkUtil.FormatUriPath("v1/identity/openidconnect/userinfo?schema={0}&access_token={1}", [
            userinfoParameters,
        ]);
        apiContext.MaskRequestId = true;
        return ConfigureAndExecute<Userinfo>(apiContext, HttpMethod.Get, resource, setAuthorizationHeader: false);
    }
}