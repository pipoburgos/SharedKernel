using SharedKernel.Application.Security;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System;

/// <summary> Date time manager. </summary>
public class ClientServerDateTime : IDateTime
{
    private readonly IIdentityService _identityService;

    /// <summary>  </summary>
    /// <param name="identityService"></param>
    public ClientServerDateTime(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary> Get utc date time. </summary>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <summary> Max date time value. </summary>
    public DateTime MaxValue => DateTime.MaxValue;

    /// <summary> Client date time value. </summary>
    public DateTime ClientNow => ConvertToClientDate(DateTime.UtcNow);

    /// <summary> Client date time value. </summary>
    public DateTime ConvertToClientDate(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Local)
            dateTime = dateTime.ToUniversalTime();

        var timezoneOffset = _identityService.GetKeyValue("TimezoneOffset");
        if (!string.IsNullOrWhiteSpace(timezoneOffset) &&
            int.TryParse(timezoneOffset, out var minutes))
            return dateTime.AddMinutes(minutes * -1);

        return dateTime;
    }
}