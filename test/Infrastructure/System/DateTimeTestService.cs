using SharedKernel.Application.System;
using System;

namespace SharedKernel.Integration.Tests.System
{
    public class DateTimeTestService : IDateTime
    {
        public DateTime Now => new DateTime(1983, 2, 24, 13, 23, 46);
        public DateTime UtcNow => new DateTime(1983, 2, 24, 13, 23, 46);
        public DateTime MaxValue => new DateTime(1983, 2, 24, 13, 23, 46);
        public DateTime ClientNow => ConvertToClientDate(new DateTime(1983, 2, 24, 13, 23, 46));

        public DateTime ConvertToClientDate(DateTime dateTime) => TimeZoneInfo.ConvertTimeFromUtc(dateTime,
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
    }
}
