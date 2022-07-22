using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace SharedKernel.Api.Middlewares
{
    internal static class ApiExceptionHandler
    {
        public static IApplicationBuilder UseSharedKernelErrors(this IApplicationBuilder app, string appName,
            string internalServerErrorMessage = "Se ha producido un error. Consulte con el administrador")
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Content-Type", "application/json;charset=utf-8");
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error == default)
                        return;

                    var serializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    var errorType = error.Error.GetType();
                    if (errorType.ToString().ToLower().StartsWith(appName))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        var errorSerialized = JsonConvert.SerializeObject(error.Error.Message, serializerSettings);
                        await context.Response.WriteAsync(errorSerialized).ConfigureAwait(false);
                        return;
                    }

                    switch (errorType.Name)
                    {
                        case nameof(AuthenticationException):
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        case nameof(UnauthorizedAccessException):
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        case "ValidationFailureException":
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            var errors = new ValidationError(error.Error as ValidationFailureException);
                            var errorsSerialized = JsonConvert.SerializeObject(errors, serializerSettings);
                            await context.Response.WriteAsync(errorsSerialized).ConfigureAwait(false);
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            await context.Response.WriteAsync(internalServerErrorMessage).ConfigureAwait(false);
                            break;
                    }

                });
            });

            return app;
        }
    }

    internal static class StringExtension
    {
        public static string ToCamelCase(this string str) =>
            string.IsNullOrEmpty(str) || str.Length < 2
                ? str
                : char.ToLowerInvariant(str[0]) + str[1..];
    }

    internal class ValidationError
    {
        public ValidationError(ValidationFailureException exception)
        {
            Errors = exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(a => a.Key.ToCamelCase(), b => b.Select(z => z.ErrorMessage).ToArray());
        }

        public Dictionary<string, string[]> Errors { get; }

        public static string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public static string Title => "One or more validation errors occurred.";

        public static int Status => 400;

        public static string TraceId => Guid.NewGuid().ToString();
    }
}
