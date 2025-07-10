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
internal class PayPalClient : IPayPalClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IOptions<PayPalOptions> _options;

    /// <summary>
    /// Initializes a new instance of <seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.IPayPalClient" /> that is used when making HTTP calls to the PayPal REST API.
    /// </summary>
    public PayPalClient(
        IHttpClientFactory clientFactory,
        IJsonSerializer jsonSerializer,
        IOptions<PayPalOptions> options)
    {
        _clientFactory = clientFactory;
        _jsonSerializer = jsonSerializer;
        _options = options;
        HttpHeaders = new Dictionary<string, string>();
    }

    /// <summary> Gets the OAuth access token to use when making API requests.. </summary>
    public PayPalTokenResponse? Token { get; private set; }

    /// <summary> Gets or sets the HTTP headers to include when making HTTP requests to the API.. </summary>
    public Dictionary<string, string> HttpHeaders { get; set; }

    /// <summary>  </summary>
    public T Send<T>(string httpMethod, string relativeUri, object? body = null)
    {
        return SendAsync<T>(httpMethod, relativeUri, body, CancellationToken.None)
            .GetAwaiter().GetResult();
    }

    /// <summary>  </summary>
    public async Task<T> SendAsync<T>(string httpMethod, string relativeUri, object? body = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage();
            var bodyString = _jsonSerializer.Serialize(body, NamingConvention.SnakeCase);
            request.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            request.Method = new HttpMethod(httpMethod);
            var client = _clientFactory.CreateClient("PayPal");

            Token ??= await GenerateOAuthToken(client);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token.TokenType, Token.AccessToken);

            request.RequestUri = new Uri(_options.Value.Settings.GetEndpoint(), relativeUri);

            var response = await client.SendAsync(request, cancellationToken);

#if NETSTANDARD2_0_OR_GREATER
            var responseContent = await response.Content.ReadAsStringAsync();
#else
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
#endif
            if (!response.IsSuccessStatusCode)
            {
                var message = $"Error en la solicitud a PayPal: {response.StatusCode}, {responseContent}";
                throw new PayPalException(message);
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
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(
            $"{_options.Value.Settings.ClientId}:{_options.Value.Settings.ClientSecret}"));

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        var requestData = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await httpClient.PostAsync(_options.Value.Settings.TokenEndpoint, requestData);

        if (!response.IsSuccessStatusCode)
            throw new PayPalException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return _jsonSerializer.Deserialize<PayPalTokenResponse>(jsonResponse, NamingConvention.SnakeCase);
    }
}