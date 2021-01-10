using System;
using SharedKernel.Application.System;

namespace SharedKernel.Infraestructure.Tests.System
{
    public class DateTimeTestService : IDateTime
    {
        public DateTime Now => new DateTime(1983, 2, 24, 13, 23, 46);
        public DateTime UtcNow => new DateTime(1983, 2, 24, 13, 23, 46);
        public DateTime MaxValue => new DateTime(1983, 2, 24, 13, 23, 46);
    }
}
