namespace SharedKernel.Infrastructure.PayPal.Exceptions;

/// <summary>
/// Represents Identity API errors related to logging into PayPal securely using PayPal login credentials.
/// <para>
/// More Information: https://developer.paypal.com/webapps/developer/docs/api/#identity
/// </para>
/// </summary>
public class IdentityException : HttpException
{
    /// <summary>
    /// Gets a <see cref="T:SharedKernel.Infrastructure.PayPal.IdentityError" /> JSON object containing the parsed details of the Identity error.
    /// </summary>
    public IdentityError Details { get; private set; }

    /// <summary>
    /// Copy constructor that attempts to deserialize the response from the specified <seealso name="HttpException" />.
    /// </summary>
    /// <param name="ex">Originating <see cref="T:SharedKernel.Infrastructure.PayPal.HttpException" /> object that contains the details of the exception.</param>
    public IdentityException(HttpException ex)
        : base(ex)
    {
        //if (string.IsNullOrEmpty(Response))
        //    return;

        //Details = JsonFormatter.ConvertFromJson<IdentityError>(Response);
        //var stringBuilder = new StringBuilder();
        //stringBuilder.AppendLine();
        //stringBuilder.AppendLine("   Error:   " + Details.Error);
        //stringBuilder.AppendLine("   Message: " + Details.ErrorDescription);
        //stringBuilder.AppendLine("   URI:     " + Details.ErrorUri);
        //LogMessage(stringBuilder.ToString());
    }

    /// <summary>
    /// Gets the prefix to use when logging the exception information.
    /// </summary>
    protected override string ExceptionMessagePrefix => "Identity Exception";
}