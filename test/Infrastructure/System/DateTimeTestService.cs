using SharedKernel.Application.System;

namespace SharedKernel.Integration.Tests.System;

public class DateTimeTestService : IDateTime
{
    public DateTime Now => new(1983, 2, 24, 13, 23, 46);
    public DateTime UtcNow => new(1983, 2, 24, 13, 23, 46);
    public DateTime MaxValue => new(1983, 2, 24, 13, 23, 46);
    public DateTime ClientNow => ConvertToClientDate(new DateTime(1983, 2, 24, 13, 23, 46));

    public DateTime ConvertToClientDate(DateTime dateTime) => TimeZoneInfo.ConvertTimeFromUtc(dateTime,
        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
}