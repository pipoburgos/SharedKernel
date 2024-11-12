using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayPal.Exceptions;
using PayPal.V1.Shared;
using SharedKernel.Application.Serializers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SharedKernel.Infrastructure.PayPal;

/// <summary>
/// IPayPalClient is used when making HTTP calls to the PayPal REST API.
/// </summary>
// ReSharper disable once InconsistentNaming
internal class PayPalClient : IPayPalClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IPayPalClient> _logger;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IOptions<PayPalOptions> _options;

    /// <summary>
    /// Initializes a new instance of <seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.IPayPalClient" /> that is used when making HTTP calls to the PayPal REST API.
    /// </summary>
    public PayPalClient(
        IHttpClientFactory clientFactory,
        ILogger<PayPalClient> logger,
        IJsonSerializer jsonSerializer,
        IOptions<PayPalOptions> options)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _jsonSerializer = jsonSerializer;
        _options = options;
        HttpHeaders = new Dictionary<string, string>();
        ResetRequestId();
    }

    /// <summary>
    /// Gets or sets the OAuth access token to use when making API requests.
    /// </summary>
    public PayPalTokenResponse? Token { get; private set; }

    /// <summary>
    /// Gets or sets the request ID used for ensuring idempotency when making a REST API call.
    /// </summary>
    public string RequestId { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP headers to include when making HTTP requests to the API.
    /// </summary>
    public Dictionary<string, string> HttpHeaders { get; set; }

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
    /// <param name="httpMethod"></param>
    /// <param name="resource"></param>
    /// <param name="payload"></param>
    /// <param name="endpoint"></param>
    /// <param name="setAuthorizationHeader"></param>
    /// <param name="maskRequestId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="PayPalException"></exception>
    /// <exception cref="PaymentsException"></exception>
    /// <exception cref="IdentityException"></exception>
    public T Send<T>(string httpMethod, string resource, object? payload = null, string? endpoint = null,
        bool setAuthorizationHeader = true, bool maskRequestId = false)
    {
        return SendAsync<T>(httpMethod, resource, payload, endpoint, CancellationToken.None)
            .GetAwaiter().GetResult();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpMethod"></param>
    /// <param name="resource"></param>
    /// <param name="payload"></param>
    /// <param name="endpoint"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="PayPalException"></exception>
    /// <exception cref="PaymentsException"></exception>
    /// <exception cref="IdentityException"></exception>
    public async Task<T> SendAsync<T>(string httpMethod, string resource, object? payload = null, string? endpoint = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage();
            var body = _jsonSerializer.Serialize(payload, NamingConvention.SnakeCase);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Method = new HttpMethod(httpMethod);
            var client = _clientFactory.CreateClient("PayPal");

            Token ??= await GenerateOAuthToken(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token.TokenType, Token.AccessToken);

            var baseUri = new Uri(_options.Value.Settings.GetEndpoint(), endpoint);
            if (!Uri.TryCreate(baseUri, resource, out _))
                throw new PayPalException("Cannot create URL; baseURI=" + baseUri + ", resourcePath=" + resource);

            request.RequestUri = new Uri(baseUri, resource);

            var response = await client.SendAsync(request, cancellationToken);

#if NETSTANDARD2_1
            var responseContent = await response.Content.ReadAsStringAsync();
#else
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
#endif
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error en la solicitud a PayPal: {StatusCode}, {responseContent}", response.StatusCode, responseContent);
                return default!;
            }

            var obj = _jsonSerializer.Deserialize<T>(responseContent, NamingConvention.SnakeCase);

            if (obj is not PayPalResource payPalResource) return obj;

            if (response.Headers.TryGetValues("PayPal-Debug-Id", out var debugIdValues))
                payPalResource.DebugId = debugIdValues.FirstOrDefault();

            return obj;
        }
        catch (ConnectionException ex)
        {
            if (ex is not HttpException) throw;

            var httpException = ex as HttpException;
            if (httpException?.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new PayPalException(ex.Message, ex);
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

    private async Task<PayPalTokenResponse> GenerateOAuthToken(HttpClient httpClient)
    {

        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.Value.Settings.ClientId}:{_options.Value.Settings.ClientSecret}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        var requestData = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await httpClient.PostAsync(_options.Value.Settings.TokenEndpoint, requestData);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return _jsonSerializer.Deserialize<PayPalTokenResponse>(jsonResponse, NamingConvention.SnakeCase);
    }
}