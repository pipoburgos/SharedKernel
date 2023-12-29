using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Api.Endpoints;

public abstract partial class EndpointBase
{
    /// <summary>
    /// Creates a <see cref="ActionResult&lt;Value&gt;"/> object that produces an <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <returns>The created <see cref="ActionResult&lt;Value&gt;"/> for the response.</returns>
    protected IActionResult OkTyped(Result<Unit> result)
    {
        return result.IsSuccess
            ? Ok()
            : BadRequest(new ValidationError(new ValidationFailureException(result.Errors.Select(e =>
                new ValidationFailure(e.PropertyName, e.ErrorMessage)).ToList())));
    }

    /// <summary>
    /// Creates a <see cref="ActionResult&lt;Value&gt;"/> object that produces an <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <returns>The created <see cref="ActionResult&lt;Value&gt;"/> for the response.</returns>
    protected ActionResult<T> OkTyped<T>(T result) => Ok(result);

    /// <summary>
    /// Creates a <see cref="IActionResult"/> object that produces an empty <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <returns>The created <see cref="IActionResult"/> for the response.</returns>
    [Obsolete("Use OkTyped(await method)")]
    protected IActionResult OkTyped() => Ok();

    /// <summary>
    /// Creates a <see cref="IActionResult"/> object that produces an empty <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <returns>The created <see cref="IActionResult"/> for the response.</returns>
    protected async Task<IActionResult> OkTyped(Task task)
    {
        await task;
        return Ok();
    }

    /// <summary>
    /// Creates a <see cref="IActionResult"/> object that produces an empty <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <returns>The created <see cref="IActionResult"/> for the response.</returns>
    protected async Task<ActionResult<T>> OkTyped<T>(Task<T> task)
    {
        return Ok(await task);
    }

    /// <summary>
    /// Creates a <see cref="IActionResult"/> object that produces an empty <see cref="StatusCodes.Status200OK"/> response.
    /// </summary>
    /// <returns>The created <see cref="IActionResult"/> for the response.</returns>
    protected async Task<IActionResult> OkTyped(Task<Result<Unit>> task)
    {
        return OkTyped(await task);
    }

}
