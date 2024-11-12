namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// Type of identification for the payment receiver.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public enum PayoutRecipientType
{
    /// <summary>Email recipient for a payout.</summary>
    Email,
    /// <summary>Phone number recipient for a payout.</summary>
    Phone,
    /// <summary>PayPal recipient for a payout.</summary>
    PaypalId,
}