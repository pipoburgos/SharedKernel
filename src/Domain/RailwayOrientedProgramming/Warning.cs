using System.Net;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

public sealed class Warning : ResultMessage
{
    public override bool IsError => false;

    public Warning(string errorMessage, string? propertyName = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.Conflict) : base(errorMessage, httpStatusCode, propertyName)
    {
    }

    public static Warning Create(string errorMessage, string? propertyName = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.Conflict)
    {
        return new Warning(errorMessage, propertyName, httpStatusCode);
    }
}