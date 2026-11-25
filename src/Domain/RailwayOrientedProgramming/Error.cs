using System.Net;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

public sealed class Error : ResultMessage
{
    public override bool IsError => true;

    public Error(string errorMessage, string? propertyName = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(errorMessage, httpStatusCode, propertyName)
    {
        if ((int)HttpStatusCode < 400)
            throw new InvalidOperationException("Invalid HTTP status code.");
    }

    public static Error Create(string errorMessage, string? propertyName = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new Error(errorMessage, propertyName, httpStatusCode);
    }
}