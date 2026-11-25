using System.Net;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

public interface IResultMessage
{
    /// <summary> The message. </summary>
    string Message { get; }

    /// <summary> To set failure flag. </summary>
    bool IsError { get; }

    /// <summary> The name of the property. </summary>
    string? PropertyName { get; }

    /// <summary> The HTTP status code. </summary>
    HttpStatusCode HttpStatusCode { get; }
}