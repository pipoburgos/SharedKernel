using PayPal.Exceptions;
using System.Net;

namespace PayPal.V1.Shared;

/// <summary>Stores details related to an HTTP response.</summary>
public class ResponseDetails
{
    /// <summary>Gets or sets the headers used in the response.</summary>
    public WebHeaderCollection? Headers { get; set; }

    /// <summary>Gets or sets the response body.</summary>
    public string? Body { get; set; }

    /// <summary>Gets or sets the response HTTP status code.</summary>
    public HttpStatusCode? StatusCode { get; set; }

    /// <summary>Gets or sets an exception related to the response.</summary>
    public ConnectionException? Exception { get; set; }

    /// <summary>
    /// Resets the state of this object and clears its properties.
    /// </summary>
    public void Reset()
    {
        Headers = null;
        Body = string.Empty;
        StatusCode = new HttpStatusCode?();
        Exception = null;
    }
}