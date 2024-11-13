using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.System;
#if NET6_0_OR_GREATER
using Microsoft.AspNetCore.Localization;
using System.Globalization;
#endif

namespace SharedKernel.Api.ServiceCollectionExtensions;

/// <summary>
/// Shared kernel api extensions
/// </summary>
public static class SharedKernelApiExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddClientServerDateTime(this IServiceCollection services)
    {
        return services
            .RemoveAll<IDateTime>()
            .AddTransient<IDateTime, ClientServerDateTime>();
    }

    /// <summary>
    /// Adds Options, Metrics, Cors, Api versioning, Api controllers, Fluent api validators and Newtonsoft to service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="policyName">The policy name of a configured policy.</param>
    /// <param name="origins">All domains who calls the api</param>
    /// <returns></returns>
    public static IServiceCollection AddSharedKernelApi(this IServiceCollection services, string policyName, string[]? origins = default)
    {
        return services
            .AddOptions()
            .AddMetrics(x => x.Configuration.Configure(o => o.Enabled = true))
            .AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder
                        .WithOrigins(origins ?? [])
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
    }

    /// <summary>
    /// Adds metrics with prometheus
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseSharedKernelMetrics(this IApplicationBuilder app)
    {
        return app
            .UseMetricServer()
            .UseHttpMetrics();
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// AddRequestLocalization.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSpanish(this IServiceCollection services)
    {
        return services
            .AddLocalization()
            .AddRequestLocalization(options =>
            {
                var supportedCultures = new[] { new CultureInfo("es") };
                options.DefaultRequestCulture = new RequestCulture("es");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            })
            .Configure<MvcOptions>(options =>
            {
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => $"El valor '{x}' es inválido.");
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(_ => "El campo tiene que ser numérico.");
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => $"El valor de la propiedad '{x}' es requerido.");
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => $"El valor '{x}' no es válido para {y}.");
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "El valor es requerido");
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => $"El valor proporcionado no es válido para {x}");
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => $"El valor '{x}' es requerido.");
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => $"El valor '{x}' es inválido.");
                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Propiedad inválida.");
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "La propiedad tiene que ser numérica.");
                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "Se requiere un cuerpo de solicitud no vacío.");
            });
    }
#endif
}