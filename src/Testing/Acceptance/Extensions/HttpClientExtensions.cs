using Newtonsoft.Json;
using SharedKernel.Testing.Acceptance.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SharedKernel.Testing.Acceptance.Extensions;

public static class HttpClientExtensions
{
    public static async Task<MultipartFormDataContent> AddFileAsync(
        this MultipartFormDataContent multipartFormDataContent, string path, string formName, string fileName,
        CancellationToken cancellationToken)
    {
        await using var plantillaStream = File.OpenRead(path);
        await using var stream = plantillaStream.ConfigureAwait(false);
        using var c = new StreamContent(plantillaStream);
        var plantillaBytes = await c.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        using var plantillaContenido = new ByteArrayContent(plantillaBytes);
        multipartFormDataContent.Add(plantillaContenido, formName, fileName);
        return multipartFormDataContent;
    }

    public static async Task<HttpResponseMessage> PostFileAsync(this HttpClient client, string url, string path,
        string formName, string fileName, CancellationToken cancellationToken)
    {
        using var multipartFormDataContent = new MultipartFormDataContent();
        await multipartFormDataContent.AddFileAsync(path, formName, fileName, cancellationToken).ConfigureAwait(false);
        return await client.PostAsync(url, multipartFormDataContent, cancellationToken).ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> PostFileAsync(this HttpClient client, string url, string path,
        string formName, string fileName, List<Tuple<string, object>> fields, CancellationToken cancellationToken)
    {
        using var multipartFormDataContent = new MultipartFormDataContent();
        fields.ForEach(field => multipartFormDataContent.Add(new StringContent(field.Item2.ToString()!), field.Item1));
        await multipartFormDataContent.AddFileAsync(path, formName, fileName, cancellationToken).ConfigureAwait(false);
        return await client.PostAsync(url, multipartFormDataContent, cancellationToken).ConfigureAwait(false);
    }

    public static StringContent Empty()
    {
        var stringContent = new StringContent(string.Empty);
        stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        return stringContent;
    }

    public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string url)
    {
        using var empty = Empty();
        return await client.PostAsync(url, empty, TestContext.Current.CancellationToken).ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient client, string url)
    {
        using var empty = Empty();
        return await client.PutAsync(url, empty, TestContext.Current.CancellationToken).ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string url)
    {
        using var empty = Empty();
        return await client.PatchAsync(url, empty, TestContext.Current.CancellationToken).ConfigureAwait(false);
    }

    public static Task<HttpResponseMessage> DeleteAsJsonAsync(this HttpClient client, string url)
    {
        return client.DeleteAsync(url, TestContext.Current.CancellationToken);
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri)
    {
        return client.PostAsync(requestUri, null, TestContext.Current.CancellationToken);
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, object request)
    {
        return client.PostAsync(requestUri, GetRequestContent(request), TestContext.Current.CancellationToken);
    }

    public static Task<HttpResponseMessage> PutAsync(this HttpClient client, string requestUri, object request)
    {
        return client.PutAsync(requestUri, GetRequestContent(request), TestContext.Current.CancellationToken);
    }

    private static StringContent GetRequestContent(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
    }

    public static HttpClient ChangeLanguage(this HttpClient client, string language)
    {
        client.DefaultRequestHeaders.AcceptLanguage.Clear();
        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
        return client;
    }

    public static async Task<dynamic> GetResponseContentAsync(this HttpResponseMessage response)
    {
        var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonConvert.DeserializeObject<dynamic>(stringResponse);
        return result!;
    }

    public static async Task<T> GetResponseContentAsync<T>(this HttpResponseMessage response)
    {
        var stringResponse = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(stringResponse);
        }

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return JsonConvert.DeserializeObject<T>(stringResponse)!;
    }

    public static Task<ErrorResponseExceptionHandler> GetErrorResponse(this HttpResponseMessage response)
    {
        return new ErrorResponseExceptionHandler(response).Build();
    }
}