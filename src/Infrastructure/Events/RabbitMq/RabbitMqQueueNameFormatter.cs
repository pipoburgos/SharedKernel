using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public static class RabbitMqQueueNameFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string Format(IRequestType information)
        {
            return information.UniqueName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string FormatRetry(IRequestType information)
        {
            return $"retry.{Format(information)}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string FormatDeadLetter(IRequestType information)
        {
            return $"dead_letter.{Format(information)}";
        }
    }
}