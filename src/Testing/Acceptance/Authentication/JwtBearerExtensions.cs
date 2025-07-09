using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SharedKernel.Testing.Acceptance.Authentication;

/// <summary> Allow to set username, roles or anything useful for faking a user. </summary>
public static class JwtBearerExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection SetFakeJwtBearerHandler(this IServiceCollection services)
    {
        return services.ChangeBearerHandlerType<FakeJwtBearerHandler>();
    }

    /// <summary> . </summary>
    public static IServiceCollection ChangeBearerHandlerType<T>(this IServiceCollection services)
        where T : AuthenticationHandler<JwtBearerOptions>
    {
        return services.Configure<AuthenticationOptions>(o =>
        {
            if (o.SchemeMap.TryGetValue(JwtBearerDefaults.AuthenticationScheme, out var value))
                value.HandlerType = typeof(T);
            //else
            //    o.AddScheme<T>(JwtBearerDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme);
        });
    }

    /// <summary> . </summary>
    public static void AddBearerToken(this HttpClient client, List<Claim> claims)
    {
        var token = JsonConvert.SerializeObject(claims.GroupBy(a => a.Type)
            .ToDictionary(a => a.Key, b => b.Select(a => a.Value)));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
    }
}
