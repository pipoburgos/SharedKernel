using System;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    public class GuidGenerator : IGuid
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        public Guid NewGuid(int value)
        {
            return new Guid($"00000000-0000-0000-0000-{value.ToString().PadLeft(12, '0')}");
        }
    }
}
