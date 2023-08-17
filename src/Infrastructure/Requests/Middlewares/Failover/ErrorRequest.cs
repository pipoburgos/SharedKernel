using SharedKernel.Domain.Entities;
using System;
using System.Globalization;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Failover;

/// <summary>  </summary>
public class ErrorRequest : Entity<string>
{
    /// <summary>  </summary>
    protected ErrorRequest()
    {
    }

    /// <summary>  </summary>
    protected ErrorRequest(string id, string request, string exception, string occurredOn = default) : base(id)
    {
        OccurredOn = occurredOn ?? DateTime.UtcNow.ToString("s", CultureInfo.CurrentCulture);
        Request = request;
        Exception = exception;
    }

    /// <summary>  </summary>
    public static ErrorRequest Create(string id, string request, string exception, string occurredOn = default)
    {
        return new ErrorRequest(id, request, exception, occurredOn);
    }

    /// <summary> When the request occurred. </summary>
    public string OccurredOn { get; }

    /// <summary> When the request occurred. </summary>
    public string Request { get; }

    /// <summary> When the request occurred. </summary>
    public string Exception { get; }
}
