using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prometheus;
using SharedKernel.Infrastructure.Validators;
using System;
#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Localization;
using System.Globalization;
#endif

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    /// <summary>
    /// Shared kernel api extensions
    /// </summary>
    public static class SharedKernelApiExtensions
    {
        /// <summary>
        /// Adds Options, Metrics, Cors, Api versioning, Api controllers, Fluent api validators and Newtonsoft to service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="policyName">The policy name of a configured policy.</param>
        /// <param name="origins">All domains who calls the api</param>
        /// <returns></returns>
        [Obsolete("Remove TValidator generic parameter")]
        public static IServiceCollection AddSharedKernelApi<TValidator>(this IServiceCollection services, string policyName, string[] origins)
        {
            services
                .AddOptions()
                .AddMetrics()
                .AddCors(options =>
                {
                    options.AddPolicy(policyName,
                        builder => builder
                            .WithOrigins(origins)
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                            .AllowCredentials());
                })
                .AddApiVersioning(config =>
                {
                    // Specify the default API Version as 1.0
                    config.DefaultApiVersion = new ApiVersion(1, 0);
                    // If the client hasn't specified the API version in the request, use the default API version number
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    // Advertise the API versions supported for the particular endpoint
                    config.ReportApiVersions = true;
                })
                .AddControllers(o =>
                {
                    o.Conventions.Add(new ControllerDocumentationConvention());
                })
                .AddFluentValidation(fv =>
                {
                    fv.AutomaticValidationEnabled = false;
                    fv.RegisterValidatorsFromAssemblyContaining<TValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<PageOptionsValidator>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            return services;
        }

        /// <summary>
        /// Adds Options, Metrics, Cors, Api versioning, Api controllers, Fluent api validators and Newtonsoft to service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="policyName">The policy name of a configured policy.</param>
        /// <param name="origins">All domains who calls the api</param>
        /// <param name="configureControllers">Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not register services used for views or pages.</param>
        /// <returns></returns>
        public static IServiceCollection AddSharedKernelApi(this IServiceCollection services, string policyName,
            string[] origins, Action<MvcOptions> configureControllers)
        {
            services
                .AddOptions()
                .AddMetrics()
                .AddCors(options =>
                {
                    options.AddPolicy(policyName,
                        builder => builder
                            .WithOrigins(origins)
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                            .AllowCredentials());
                })
                .AddApiVersioning(config =>
                {
                    // Specify the default API Version as 1.0
                    config.DefaultApiVersion = new ApiVersion(1, 0);
                    // If the client hasn't specified the API version in the request, use the default API version number
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    // Advertise the API versions supported for the particular endpoint
                    config.ReportApiVersions = true;
                })
                .AddControllers(configureControllers)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            return services;
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

#if NET5_0_OR_GREATER
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
}
