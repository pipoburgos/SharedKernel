using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace SharedKernel.Testing.Acceptance.Authentication;

public class FakeJwtBearerHandler : AuthenticationHandler<JwtBearerOptions>
{
    public static readonly string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

#if NET8_0_OR_GREATER
    public FakeJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }
#else
    public FakeJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) { }
#endif

    /// <summary>
    /// The handler calls methods on the events which give the application control at certain points where processing is occurring.
    /// If it is not provided a default instance is supplied which does nothing when the methods are called.
    /// </summary>
    private new JwtBearerEvents Events
    {
        get
        {
            if (base.Events is JwtBearerEvents)
            {
                return (base.Events as JwtBearerEvents)!;
            }
            base.Events = new JwtBearerEvents();
            return (base.Events as JwtBearerEvents)!;
        }
    }

    /// <summary> Searches the 'Authorization' header for a 'Bearer' token. </summary>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);

            await Events.MessageReceived(messageReceivedContext);

            var token = messageReceivedContext.Token;

            if (string.IsNullOrEmpty(token))
            {
                string? authorization = Request.Headers["Authorization"];


                if (string.IsNullOrEmpty(authorization))
                    return AuthenticateResult.NoResult();

                if (authorization.StartsWith(AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                    token = authorization.Substring($"{AuthenticationScheme} ".Length).Trim();

                if (string.IsNullOrEmpty(token))
                    return AuthenticateResult.NoResult();
            }

            var tokenDecoded = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(token);

            var claimsIdentity = new ClaimsIdentity(AuthenticationScheme, ClaimTypes.Sid, ClaimTypes.Role);

            foreach (var td in tokenDecoded!)
            {
                switch (td.Value)
                {
                    case string:
                        claimsIdentity.AddClaim(new Claim(td.Key, td.Value));
                        break;
                    case IEnumerable:
                        {
                            foreach (string subValue in td.Value)
                            {
                                claimsIdentity.AddClaim(new Claim(td.Key, subValue));
                            }

                            break;
                        }
                }
            }

            var principal = new ClaimsPrincipal(claimsIdentity);

            var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
            {
                Principal = principal,
            };

            await Events.TokenValidated(tokenValidatedContext);

            if (Options.SaveToken)
            {
                tokenValidatedContext.Properties.StoreTokens(new List<AuthenticationToken>
                    { new() { Name = "access_token", Value = token } });
            }

            tokenValidatedContext.Success();
            return tokenValidatedContext.Result!;
        }
        catch (Exception ex)
        {
            var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
            {
                Exception = ex,
            };

            await Events.AuthenticationFailed(authenticationFailedContext);
            return authenticationFailedContext.Result!;
        }
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var authResult = await HandleAuthenticateOnceSafeAsync();
        var eventContext = new JwtBearerChallengeContext(Context, Scheme, Options, properties)
        {
            AuthenticateFailure = authResult.Failure!,
        };

        await Events.Challenge(eventContext);
        if (eventContext.Handled)
        {
            return;
        }

        Response.StatusCode = 401;

        if (string.IsNullOrEmpty(eventContext.Error) &&
            string.IsNullOrEmpty(eventContext.ErrorDescription) &&
            string.IsNullOrEmpty(eventContext.ErrorUri))
        {
            Response.Headers.Append(HeaderNames.WWWAuthenticate, Options.Challenge);
            return;
        }

        // https://tools.ietf.org/html/rfc6750#section-3.1
        // WWW-Authenticate: Bearer realm="example", error="invalid_token", error_description="The access token expired"
        var builder = new StringBuilder(Options.Challenge);
        if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0)
        {
            // Only add a comma after the first param, if any
            builder.Append(',');
        }
        if (!string.IsNullOrEmpty(eventContext.Error))
        {
            builder.Append(" error=\"");
            builder.Append(eventContext.Error);
            builder.Append("\"");
        }
        if (!string.IsNullOrEmpty(eventContext.ErrorDescription))
        {
            if (!string.IsNullOrEmpty(eventContext.Error))
            {
                builder.Append(",");
            }

            builder.Append(" error_description=\"");
            builder.Append(eventContext.ErrorDescription);
            builder.Append('\"');
        }
        if (!string.IsNullOrEmpty(eventContext.ErrorUri))
        {
            if (!string.IsNullOrEmpty(eventContext.Error) ||
                !string.IsNullOrEmpty(eventContext.ErrorDescription))
            {
                builder.Append(",");
            }

            builder.Append(" error_uri=\"");
            builder.Append(eventContext.ErrorUri);
            builder.Append('\"');
        }

        Response.Headers.Append(HeaderNames.WWWAuthenticate, builder.ToString());
    }
}
