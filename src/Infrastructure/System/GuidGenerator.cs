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
    }
}
