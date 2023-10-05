using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SharedKernel.Testing.Acceptance.Authentication;

/// <summary> Allow to set username, roles or anything useful for faking a user. </summary>
public static class HttpClientExtensions
{
    /// <summary> Define a Token with a custom object. </summary>
    public static HttpClient SetFakeBearerToken(this HttpClient client, ClaimsIdentity token)
    {
        client.SetToken(FakeJwtBearerDefaults.AuthenticationScheme,
            JsonConvert.SerializeObject(token.Claims.GroupBy(a => a.Type)
                .ToDictionary(a => a.Key, b => b.Select(a => a.Value))));

        return client;
    }

    /// <summary> Define a Token with a custom object. </summary>
    public static HttpClient SetFakeBearerToken(this HttpClient client, object token)
    {
        client.SetToken(FakeJwtBearerDefaults.AuthenticationScheme, JsonConvert.SerializeObject(token));

        return client;
    }

    /// <summary> Define a Token with juste a Username. </summary>
    public static HttpClient SetFakeBearerToken(this HttpClient client, string username)
    {
        client.SetFakeBearerToken(new
        {
            sub = username
        });

        return client;
    }

    /// <summary> Define a Token with a Username and some roles. </summary>
    public static HttpClient SetFakeBearerToken(this HttpClient client, string username, string[] roles)
    {

        client.SetFakeBearerToken(new
        {
            sub = username,
            role = roles
        });

        return client;
    }

    /// <summary> Set Raw Token. </summary>
    public static HttpClient SetToken(this HttpClient client, string scheme, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

        return client;
    }
}
