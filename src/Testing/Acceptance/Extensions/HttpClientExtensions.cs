using Newtonsoft.Json;
using SharedKernel.Testing.Acceptance.Exceptions;
using System.Net;
using System.Net.Http.Headers;

namespace SharedKernel.Testing.Acceptance.Extensions;

public static class HttpClientExtensions
{
    public static async Task<MultipartFormDataContent> AddFileAsync(
        this MultipartFormDataContent multipartFormDataContent, string path, string? formName = default,
        string? fileName = default, CancellationToken cancellationToken = default)
    {
        await using var plantillaStream = File.OpenRead(path);
        var plantillaBytes = await new StreamContent(plantillaStream).ReadAsByteArrayAsync(cancellationToken);
        var plantillaContenido = new ByteArrayContent(plantillaBytes);
        multipartFormDataContent.Add(plantillaContenido, formName ?? "file", fileName ?? "Plantilla.dotx");

        return multipartFormDataContent;
    }

    public static StringContent Empty()
    {
        var stringContent = new StringContent(string.Empty);
        stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        return stringContent;
    }

    public static async Task<HttpResponseMessage> PostFileAsync(this HttpClient client, string url, string path, string? formName = default,
        string? fileName = default)
    {
        var multipartFormDataContent = new MultipartFormDataContent();
        await multipartFormDataContent.AddFileAsync(path, formName, fileName);
        return await client.PostAsync(url, multipartFormDataContent);
    }

    public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string url)
    {
        return client.PostAsync(url, Empty());
    }

    public static Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient client, string url)
    {
        return client.PutAsync(url, Empty());
    }

    public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string url)
    {
        return client.PatchAsync(url, Empty());
    }

    public static Task<HttpResponseMessage> DeleteAsJsonAsync(this HttpClient client, string url)
    {
        return client.DeleteAsync(url);
    }

    public static async Task<dynamic> GetResponseContentAsync(this HttpResponseMessage response)
    {
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<dynamic>(stringResponse);
        return result!;
    }

    public static async Task<T> GetResponseContentAsync<T>(this HttpResponseMessage response)
    {
        var stringResponse = await response.Content.ReadAsStringAsync();
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