using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SharedKernel.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace BankAccounts.Api.Shared
{
    /// <summary> Errors middleware. </summary>
    internal static class ApiExceptionHandler
    {
        public static IApplicationBuilder UseApiErrors(this IApplicationBuilder app)
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

                    if (error.Error.GetType().ToString().StartsWith("BankAccounts"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        return;
                    }

                    switch (error.Error.GetType().ToString())
                    {
                        case nameof(AuthenticationException):
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        case "eReges.Domain.Shared.EregesException":
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            break;
                        case "SharedKernel.Application.Validator.ValidationFailureException":
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            var validationErrors = new ValidationError(error.Error as ValidationFailureException);
                            var errorsText = JsonConvert.SerializeObject(validationErrors);
                            await context.Response.WriteAsync(errorsText).ConfigureAwait(false);
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            await context.Response
                                .WriteAsync("Se ha producido un error. Consulte con el administrador")
                                .ConfigureAwait(false);
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

        public string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public string Title => "One or more validation errors occurred.";

        public int Status => 400;

        public string TraceId => Guid.NewGuid().ToString();
    }
}
