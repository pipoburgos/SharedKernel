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
            SupportedUICultures = supported
        });

        app.Use((context, next) =>
        {
            if (supportedCultures.Any() && context.Request.GetTypedHeaders().AcceptLanguage.Any())
            {
                var requestCulture = context.Request.GetTypedHeaders()
                    .AcceptLanguage
                    .OrderByDescending(x => x.Quality ?? 1) // Quality defines priority from 0 to 1, where 1 is the highest.
                    .Select(x => x.Value.ToString())
                    .FirstOrDefault(supportedCultures.Contains);

                if (requestCulture == default)
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    context.Response.WriteAsJsonAsync("Invalid Accept-Language values.");
                    return Task.CompletedTask;
                }

                defaultLanguage = new CultureInfo(requestCulture);
            }

            Thread.CurrentThread.CurrentCulture = defaultLanguage;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return next();
        });

        return app;
    }
}
