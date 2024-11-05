namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Event arguments for when an error is encountered while deserializing a JSON string.
/// </summary>
public class JsonFormatterDeserializationErrorEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the error message associated with this event.
    /// </summary>
    public string Message { get; set; }
}