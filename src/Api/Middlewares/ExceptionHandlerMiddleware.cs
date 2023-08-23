using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace SharedKernel.Api.Middlewares;

/// <summary> Exception handler middleware. </summary>
public static class ExceptionHandlerMiddleware
{
    /// <summary> Exception handler middleware.  </summary>
    /// <param name="app">Aplication</param>
    /// <param name="appName">Set not acceptable to exceptions who starts with this app name.</param>
    /// <param name="unhandledExceptionAction">Set the default message to unknown exceptions. </param>
    /// <param name="debugAction">To debug api errors. </param>
    /// <returns></returns>
    public static IApplicationBuilder UseSharedKernelExceptionHandler(this IApplicationBuilder app, string appName,
        Func<IExceptionHandlerFeature, string> unhandledExceptionAction,
        Action<IExceptionHandlerFeature> debugAction = default)
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

                debugAction?.Invoke(error);

                var jsonSerializer = context.RequestServices.GetRequiredService<IJsonSerializer>();

                var errorType = error.Error.GetType();
                if (errorType.Namespace != null && errorType.Namespace.Split(".").First().ToLower().StartsWith(appName))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    var errorSerialized = jsonSerializer.Serialize(error.Error.Message);
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
                        var errorsSerialized2 = jsonSerializer.Serialize(error.Error);
                        await context.Response.WriteAsync(errorsSerialized2).ConfigureAwait(false);
                        break;
                    case nameof(ValidationFailureException):
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        var errors = new ValidationError(error.Error as ValidationFailureException);
                        var errorsSerialized = jsonSerializer.Serialize(errors);
                        await context.Response.WriteAsync(errorsSerialized).ConfigureAwait(false);
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var genericError = jsonSerializer
                            .Serialize(unhandledExceptionAction(error));
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

    public static string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

    public static string Title => "One or more validation errors occurred.";

    public static int Status => 400;

    public static string TraceId => Guid.NewGuid().ToString();
}
