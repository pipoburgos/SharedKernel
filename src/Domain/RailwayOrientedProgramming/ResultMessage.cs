using System.Net;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
public abstract class ResultMessage : IResultMessage
{
    /// <summary> . </summary>
    protected ResultMessage(
        string message,
        HttpStatusCode httpStatusCode,
        string? propertyName = null)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
        PropertyName = propertyName;
    }

    /// <summary> . </summary>
    public string Message { get; private set; }

    /// <summary> . </summary>
    public abstract bool IsError { get; }

    /// <summary> . </summary>
    public string? PropertyName { get; private set; }

    /// <summary> . </summary>
    public HttpStatusCode HttpStatusCode { get; private set; }

    /// <summary> . </summary>
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(PropertyName) ? Message : $"{PropertyName}: {Message}";
    }
}