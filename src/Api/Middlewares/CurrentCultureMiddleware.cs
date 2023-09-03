using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace SharedKernel.Api.Middlewares;

/// <summary> Current culture middleware. </summary>
public static class CurrentCultureMiddleware
{
    /// <summary> Configure Accept-Language header with english, spanish and chinese cultures. </summary>
    public static IApplicationBuilder UseSharedKernelCurrentCulture(this IApplicationBuilder app,
        params string[] supportedCultures)
    {
        var defaultCulture = new CultureInfo("en-US");
        var supported = supportedCultures.Select(x => new CultureInfo(x)).ToList();

        var defaultLanguage = supported.Any() ? supported.First() : defaultCulture;

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultLanguage),
            SupportedCultures = supported,
            SupportedUICultures = supported
        });

        app.Use((context, next) =>
        {
            if (supportedCultures.Any())
            {
                var userLanguages = context.Request.Headers["Accept-Language"].ToString();

                if (userLanguages != default!)
                {
                    var requestCultures = userLanguages.Contains(',')
                        ? userLanguages.Split(',').ToList()
                        : new List<string> { userLanguages };

                    string? requestCultur = default;
                    foreach (var culture in requestCultures)
                    {
                        if (!supportedCultures.Contains(culture))
                            break;

                        requestCultur = culture;
                    }

                    if (requestCultur == default)
                    {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                        context.Response.WriteAsJsonAsync("Invalud Accept-Language values.");
                        return Task.CompletedTask;
                    }

                    defaultLanguage = new CultureInfo(requestCultur);
                }
            }

            Thread.CurrentThread.CurrentCulture = defaultLanguage;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return next();
        });

        return app;
    }
}
