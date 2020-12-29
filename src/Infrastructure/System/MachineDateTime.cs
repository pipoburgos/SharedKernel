using System;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public class MachineDateTime : IDateTime
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        public DateTime MaxValue => DateTime.MaxValue;
    }
}
