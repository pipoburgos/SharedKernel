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
        var supported = supportedCultures.Select(x => new CultureInfo(x)).ToList();

        var defaultLanguage = supported.Any() ? supported.First() : new CultureInfo("en-US");

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultLanguage),
            SupportedCultures = supported,
            SupportedUICultures = supported,
        });

        app.Use(async (context, next) =>
        {
            if (supportedCultures.Any() && context.Request.GetTypedHeaders().AcceptLanguage.Any())
            {
                var requestCulture = context.Request.GetTypedHeaders()
                    .AcceptLanguage
                    .OrderByDescending(x => x.Quality ?? 1)
                    .Select(x => x.Value.ToString())
                    .FirstOrDefault(supportedCultures.Contains);

                if (requestCulture == default)
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync("Invalid Accept-Language values.");
                    return;
                }

                defaultLanguage = new CultureInfo(requestCulture);
            }

            Thread.CurrentThread.CurrentCulture = defaultLanguage;
            Thread.CurrentThread.CurrentUICulture = defaultLanguage;

            await next();
        });

        return app;
    }
}
