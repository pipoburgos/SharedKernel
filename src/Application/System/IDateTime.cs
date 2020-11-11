using System;

namespace SharedKernel.Application.System
{
    public interface IDateTime
    {
        //DateTime Now { get; }

        DateTime UtcNow { get; }

        DateTime MaxValue { get; }
    }
}
