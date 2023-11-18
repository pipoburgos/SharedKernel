using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Api.Endpoints;

/// <summary> Endpoint base. </summary>
[ApiController, Produces("application/json")]
public abstract partial class EndpointBase : ControllerBase
{
    /// <summary> Gets the command bus. </summary>
    protected ICommandBus CommandBus => HttpContext.RequestServices.GetRequiredService<ICommandBus>();

    /// <summary> Gets de query bus. </summary>
    protected IQueryBus QueryBus => HttpContext.RequestServices.GetRequiredService<IQueryBus>();

    /// <summary>
    /// Read a file an return in streaming
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    protected async Task Streaming(string filePath, CancellationToken cancellationToken)
    {
        Response.StatusCode = 200;
        Response.Headers.Append(HeaderNames.ContentDisposition, $"attachment; filename=\"{Path.GetFileName(filePath)}\"");
        Response.Headers.Append(HeaderNames.ContentType, "application/octet-stream");
        await using var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var outputStream = Response.Body;
        const int bufferSize = 1 << 10;
        var buffer = new byte[bufferSize];
        while (true)
        {
            var bytesRead = await inputStream.ReadAsync(buffer, 0, bufferSize, cancellationToken);
            if (bytesRead == 0)
                break;

            await outputStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
        }
        await outputStream.FlushAsync(cancellationToken);
    }
}
