using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.PayPal.Exceptions;
using System.Net;
using System.Text;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// IPayPalClient is used when making HTTP calls to the PayPal REST API.
/// </summary>
// ReSharper disable once InconsistentNaming
public class APIContext : IPayPalClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IPayPalClient> _logger;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IOptions<PayPalOptions> _options;

    /// <summary>
    /// Initializes a new instance of <seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.IPayPalClient" /> that is used when making HTTP calls to the PayPal REST API.
    /// </summary>
    public APIContext(IHttpClientFactory clientFactory, ILogger<IPayPalClient> logger, IJsonSerializer jsonSerializer, IOptions<PayPalOptions> options)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _jsonSerializer = jsonSerializer;
        _options = options;
        HttpHeaders = new Dictionary<string, string>();
        ResetRequestId();
        //SdkVersion = new SdkVersion();
    }

    ///// <summary>
    ///// Initializes a new instance of <seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.IPayPalClient" /> that is used when making HTTP calls to the PayPal REST API; as well as sets and verifies the state of an <paramref name="accessToken" />.
    ///// </summary>
    ///// <param name="accessToken">OAuth access token to use when making API requests</param>
    //public IPayPalClient(string accessToken)
    //    : this()
    //{
    //    AccessToken = !string.IsNullOrEmpty(accessToken) ? accessToken : throw new ArgumentNullException(nameof(accessToken), "accessToken cannot be null");
    //}

    ///// <summary>
    ///// Initializes a new instance of <seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.IPayPalClient" /> that is used when making HTTP calls to the PayPal REST API; as well as sets and verifies the states of an <paramref name="accessToken" /> and <paramref name="requestId" />.
    ///// </summary>
    ///// <param name="accessToken">OAuth access token to use when making API requests</param>
    ///// <param name="requestId">ID used for ensuring idempotency when making a REST API call</param>
    //public IPayPalClient(string accessToken, string requestId)
    //    : this(accessToken)
    //{
    //    RequestId = !string.IsNullOrEmpty(requestId) ? requestId : throw new ArgumentNullException(nameof(requestId), "requestId cannot be null");
    //}

    /// <summary>
    /// Gets or sets the OAuth access token to use when making API requests.
    /// </summary>
    public string? AccessToken { get; private set; }

    /// <summary>
    /// Gets or sets whether or not the PayPal-Request-Id header will be set when making API requests, which is used for ensuring idempotency when making API calls.
    /// </summary>
    public bool MaskRequestId { get; set; }

    /// <summary>
    /// Gets or sets the request ID used for ensuring idempotency when making a REST API call.
    /// </summary>
    public string RequestId { get; private set; } = null!;

    ///// <summary>
    ///// Gets or sets the PayPal configuration settings to be used when making API requests.
    ///// </summary>
    //public Dictionary<string, string> Config { get; set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP headers to include when making HTTP requests to the API.
    /// </summary>
    public Dictionary<string, string> HttpHeaders { get; set; }

    ///// <summary>
    ///// Gets or sets the SDK version to include in the User-Agent header.
    ///// </summary>
    //public SdkVersion SdkVersion { get; set; }

    /// <summary>
    /// Resets the request ID used for ensuring idempotency when making a REST API call.
    /// </summary>
    public void ResetRequestId()
    {
        RequestId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddHeader(string key, string value)
    {
        HttpHeaders.Add(key, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpMethod"></param>
    /// <param name="resource"></param>
    /// <param name="payload"></param>
    /// <param name="endpoint"></param>
    /// <param name="setAuthorizationHeader"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="PayPalException"></exception>
    /// <exception cref="PaymentsException"></exception>
    /// <exception cref="IdentityException"></exception>
    public async Task<T> Send<T>(HttpMethod httpMethod, string resource, object? payload = null, string? endpoint = null,
        bool setAuthorizationHeader = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var headerMap = GetHeaderMap();
            if (!setAuthorizationHeader && headerMap.ContainsKey("Authorization"))
                headerMap.Remove("Authorization");

            var request = new HttpRequestMessage();
            request.Content = new StringContent(_jsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            request.Method = httpMethod;
            if (headerMap.ContainsKey("Content-Type"))
            {
                request.Content.Headers.Add("Content-Type", headerMap["Content-Type"].Trim());
                headerMap.Remove("Content-Type");
            }
            else
            {
                request.Content.Headers.Add("Content-Type", "application/json");
            }

            if (headerMap.ContainsKey("User-Agent"))
            {
                var encoding = Encoding.GetEncoding("iso-8859-1", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
                var bytes = Encoding.Convert(Encoding.UTF8, encoding, Encoding.UTF8.GetBytes(headerMap["User-Agent"]));
                request.Content.Headers.Add("User-Agent", encoding.GetString(bytes));
                headerMap.Remove("User-Agent");
            }

            foreach (var keyValuePair in headerMap)
                request.Content.Headers.Add(keyValuePair.Key, keyValuePair.Value);

            foreach (var header in request.Content.Headers)
                _logger.LogTrace($"{header.Key} : {string.Join(", ", header.Value)}");

            var client = _clientFactory.CreateClient("PayPal");

            var baseUri = new Uri(_options.Value.Settings.GetEndpoint(), endpoint);
            if (!Uri.TryCreate(baseUri, resource, out _))
                throw new PayPalException("Cannot create URL; baseURI=" + baseUri + ", resourcePath=" + resource);

            request.RequestUri = new Uri(baseUri, resource);

            var response = await client.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error en la solicitud a PayPal: {StatusCode}", response.StatusCode);
                return default!;
            }

#if NETSTANDARD2_1
            var responseContent = await response.Content.ReadAsStringAsync();
#else
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

            var obj = _jsonSerializer.Deserialize<T>(responseContent, NamingConvention.SnakeCase);

            if (obj is PayPalResource payPalResource)
            {
                if (response.Headers.TryGetValues("PayPal-Debug-Id", out var debugIdValues))
                {
                    payPalResource.DebugId = debugIdValues.FirstOrDefault();
                }
            }

            return obj;
        }
        catch (ConnectionException ex)
        {
            if (ex is not HttpException) throw;

            var httpException = ex as HttpException;
            if (httpException?.StatusCode == HttpStatusCode.BadRequest)
            {
                if (httpException.TryConvertTo(out PaymentsException other))
                    throw other;
            }
            else
            {
                if (httpException?.StatusCode == HttpStatusCode.Unauthorized &&
                    httpException.TryConvertTo(out IdentityException other))
                    throw other;
            }
            throw;
        }
        catch (PayPalException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PayPalException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Gets a collection of headers to be used in an HTTP request.
    /// </summary>
    /// <returns>A collection of headers.</returns>
    public Dictionary<string, string> GetHeaderMap()
    {
        var headerMap = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(AccessToken))
        {
            headerMap["Authorization"] = AccessToken;
        }
        else
        {
            var base64 = EncodeToBase64(_options.Value.Settings.ClientId, _options.Value.Settings.ClientSecret);
            headerMap["Authorization"] = "Basic " + base64;
        }
        if (!MaskRequestId && !string.IsNullOrEmpty(RequestId))
            headerMap["PayPal-Request-Id"] = RequestId;
        var header = UserAgentHeader.GetHeader();
        if (header.Count > 0)
            foreach (var keyValuePair in header)
                headerMap[keyValuePair.Key] = keyValuePair.Value;
        if (HttpHeaders.Count > 0)
            foreach (var httpHeader in HttpHeaders)
                headerMap[httpHeader.Key] = httpHeader.Value;

        return headerMap;
    }

    /// <summary>
    /// Covnerts the specified client credentials to a base-64 string for authorization purposes.
    /// </summary>
    /// <param name="clientId">The client ID to be used in generating the base-64 client identifier.</param>
    /// <param name="clientSecret">The client secret to be used in generating the base-64 client identifier.</param>
    /// <returns>The base-64 encoded client identifier to use in the authorization request.</returns>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.MissingCredentialException">Thrown if clientId or clientSecret are null or empty.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.InvalidCredentialException">Thrown if there is an issue converting the credentials to a formatted authorization string.</exception>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PayPalException">Thrown for any other issue encountered. See inner exception for further details.</exception>
    private static string EncodeToBase64(string? clientId, string? clientSecret)
    {
        if (string.IsNullOrEmpty(clientId))
            throw new MissingCredentialException("clientId is missing.");
        if (string.IsNullOrEmpty(clientSecret))
            throw new MissingCredentialException("clientSecret is missing.");

        try
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case FormatException _:
                case ArgumentNullException _:
                    throw new InvalidCredentialException(
                        $"Unable to convert client credentials to base-64 string.\n  clientId: {clientId}\n  clientSecret: {clientSecret}\n  Error: {ex.Message}");
                default:
                    throw new PayPalException(ex.Message, ex);
            }
        }
    }
}