namespace SharedKernel.Infrastructure.PayPal.Exceptions;

/// <summary>
/// Represents an error that occurred in the PayPal SDK when application credentials are in an invalid state.
/// </summary>
public class InvalidCredentialException : PayPalException
{
    /// <summary>
    /// Represents errors where certain credential information is invalid.
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public InvalidCredentialException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Gets the prefix to use when logging the exception information.
    /// </summary>
    protected override string ExceptionMessagePrefix => "Invalid Credential";
}