using System.Net;

namespace PayPal.V1.Shared;

/// <summary>Stores details related to an HTTP request.</summary>
public class RequestDetails
{
    /// <summary>Gets or sets the URL for the request.</summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method verb used for the request.
    /// </summary>
    public string? Method { get; set; }

    /// <summary>Gets or sets the headers used in the request.</summary>
    public WebHeaderCollection? Headers { get; set; }

    /// <summary>Gets or sets the request body.</summary>
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets the number of retry attempts for sending an HTTP request.
    /// </summary>
    public int RetryAttempts { get; set; }

    /// <summary>
    /// Resets the state of this object and clears its properties.
    /// </summary>
    public void Reset()
    {
        Url = string.Empty;
        Headers = null;
        Body = string.Empty;
        RetryAttempts = 0;
    }
}