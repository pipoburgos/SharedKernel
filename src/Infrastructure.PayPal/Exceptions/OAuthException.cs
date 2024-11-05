namespace SharedKernel.Infrastructure.PayPal.Exceptions;

/// <summary>
/// Represents an error that occurred when requesting an OAuth token.
/// </summary>
public class OAuthException : PayPalException
{
    /// <summary>
    /// Represents an exception related to requesting an OAuth token.
    /// </summary>
    /// <param name="message">The message associated with the exception.</param>
    public OAuthException(string message)
        : this(message, null)
    {
    }

    /// <summary>
    /// Represents an exception related to requesting an OAuth token.
    /// </summary>
    /// <param name="message">The message associated with the exception.</param>
    /// <param name="exception">More exception information that should be included with this exception.</param>
    public OAuthException(string message, Exception exception)
        : base(message, exception)
    {
    }

    /// <summary>Gets the OAuth exception short message</summary>
    public string OAuthExceptionShortMessage => Message;

    /// <summary>Gets the OAuth exception long message</summary>
    public string OAuthExceptionLongMessage
    {
        get => InnerException != null ? InnerException.Message : string.Empty;
    }

    /// <summary>
    /// Gets the prefix to use when logging the exception information.
    /// </summary>
    protected override string ExceptionMessagePrefix => "OAuth Exception";
}