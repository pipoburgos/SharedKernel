using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>  </summary>
    public class MachineDateTime : IDateTime
    {
        /// <summary>  </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>  </summary>
        public DateTime MaxValue => DateTime.MaxValue;

        /// <summary>  </summary>
        public DateTime ClientNow => DateTime.Now;

        /// <summary>  </summary>
        public DateTime ConvertToClientDate(DateTime dateTime)
        {
            switch (dateTime.Kind)
            {
                case DateTimeKind.Local:
                    return dateTime;
                case DateTimeKind.Utc:
                    return dateTime.ToLocalTime();
                case DateTimeKind.Unspecified:
                    return dateTime;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
