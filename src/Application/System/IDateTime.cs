using System;

namespace SharedKernel.Application.System
{
    /// <summary>
    /// Date time manager
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Machine utc date time
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Max date time value
        /// </summary>
        DateTime MaxValue { get; }
    }
}
