using System;

namespace SharedKernel.Application.System
{
    public interface IGuid
    {
        Guid NewGuid();

        Guid NewGuid(int value);
    }
}
