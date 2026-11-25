using System.Net;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

public sealed class Ok : ResultMessage
{
    public override bool IsError => false;

    public Ok(string errorMessage, string? propertyName = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK) :
        base(errorMessage, httpStatusCode, propertyName)
    {
        if ((int)HttpStatusCode < 200 || (int)HttpStatusCode > 299)
            throw new InvalidOperationException("Invalid HTTP status code.");
    }

    public static Ok Create(string errorMessage, string? propertyName = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.Conflict)
    {
        return new Ok(errorMessage, propertyName, httpStatusCode);
    }
}