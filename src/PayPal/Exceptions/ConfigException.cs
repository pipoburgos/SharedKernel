namespace PayPal.Exceptions;

/// <summary>
/// Represents an error that occurred in the PayPal SDK when attempting to load information from the application's config file.
/// </summary>
public class ConfigException : PayPalException
{
    /// <summary>
    /// Represents errors that are related to the application's configuration.
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public ConfigException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Gets the prefix to use when logging the exception information.
    /// </summary>
    protected override string ExceptionMessagePrefix => "Configuration Exception";
}