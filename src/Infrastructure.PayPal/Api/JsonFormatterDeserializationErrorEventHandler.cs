namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Event handler delegate for when an error is encountered while deserializing a JSON string.
/// </summary>
/// <param name="e"></param>
public delegate void JsonFormatterDeserializationErrorEventHandler(
    JsonFormatterDeserializationErrorEventArgs e);