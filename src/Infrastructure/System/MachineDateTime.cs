using System;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    public class MachineDateTime : IDateTime
    {
        //public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime MaxValue => DateTime.MaxValue;
    }
}
