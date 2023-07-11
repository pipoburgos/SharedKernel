using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.Validator;
using Swashbuckle.AspNetCore.SwaggerGen;
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

                    var exception = error.Error;

                    if (exception.GetType() == typeof(FallbackException))
                        exception = error.Error.InnerException;

                    if (exception == default)
                        return;

                    var serializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    var errorType = exception.GetType();
                    if (errorType.ToString().ToLower().StartsWith("bankaccount"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        var errorSerialized = JsonConvert.SerializeObject(exception.Message, serializerSettings);
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
                        case nameof(SwaggerGeneratorException):
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            var errorsSerialized2 = JsonConvert.SerializeObject(exception, serializerSettings);
                            await context.Response.WriteAsync(errorsSerialized2).ConfigureAwait(false);
                            break;
                        case nameof(ValidationFailureException):
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            var errors = new ValidationError(exception as ValidationFailureException);
                            var errorsSerialized = JsonConvert.SerializeObject(errors, serializerSettings);
                            await context.Response.WriteAsync(errorsSerialized).ConfigureAwait(false);
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            var genericError = JsonConvert.SerializeObject($"An error has occurred, check with the administrator ({exception.Message})", serializerSettings);
                            await context.Response
                                .WriteAsync(genericError)
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
