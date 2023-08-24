using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public class GuidGenerator : IGuid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Guid NewGuid(int value)
        {
            return new Guid($"00000000-0000-0000-0000-{value.ToString().PadLeft(12, '0')}");
        }
    }
}
