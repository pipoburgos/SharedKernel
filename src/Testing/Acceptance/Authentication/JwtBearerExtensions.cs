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
    public static IServiceCollection SetFakeJwtBearerHandler(this IServiceCollection services)
    {
        return services.ChangeBearerHandlerType<FakeJwtBearerHandler>();
    }

    public static IServiceCollection ChangeBearerHandlerType<T>(this IServiceCollection services)
        where T : AuthenticationHandler<JwtBearerOptions>
    {
        return services.Configure<AuthenticationOptions>(o =>
            o.SchemeMap[JwtBearerDefaults.AuthenticationScheme].HandlerType = typeof(T));
    }

    public static void AddBearerToken(this HttpClient client, List<Claim> claims)
    {
        var token = JsonConvert.SerializeObject(claims.GroupBy(a => a.Type)
            .ToDictionary(a => a.Key, b => b.Select(a => a.Value)));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
    }
}
