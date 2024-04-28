using Newtonsoft.Json;
using SharedKernel.Testing.Acceptance.Exceptions;
using System.Net;
using System.Net.Http.Headers;

namespace SharedKernel.Testing.Acceptance.Extensions;

public static class HttpClientExtensions
{
    public static async Task<MultipartFormDataContent> AddFileAsync(
        this MultipartFormDataContent multipartFormDataContent, string path, string formName, string fileName,
        CancellationToken cancellationToken)
    {
        await using var plantillaStream = File.OpenRead(path);
        var plantillaBytes = await new StreamContent(plantillaStream).ReadAsByteArrayAsync(cancellationToken);
        var plantillaContenido = new ByteArrayContent(plantillaBytes);
        multipartFormDataContent.Add(plantillaContenido, formName, fileName);
        return multipartFormDataContent;
    }

    public static async Task<HttpResponseMessage> PostFileAsync(this HttpClient client, string url, string path,
        string formName, string fileName, CancellationToken cancellationToken)
    {
        var multipartFormDataContent = new MultipartFormDataContent();
        await multipartFormDataContent.AddFileAsync(path, formName, fileName, cancellationToken);
        return await client.PostAsync(url, multipartFormDataContent, cancellationToken);
    }

    public static async Task<HttpResponseMessage> PostFileAsync(this HttpClient client, string url, string path,
        string formName, string fileName, List<Tuple<string, object>> fields, CancellationToken cancellationToken)
    {
        var multipartFormDataContent = new MultipartFormDataContent();
        fields.ForEach(field => multipartFormDataContent.Add(new StringContent(field.Item2.ToString()!), field.Item1));
        await multipartFormDataContent.AddFileAsync(path, formName, fileName, cancellationToken);
        return await client.PostAsync(url, multipartFormDataContent, cancellationToken);
    }

    public static StringContent Empty()
    {
        var stringContent = new StringContent(string.Empty);
        stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        return stringContent;
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