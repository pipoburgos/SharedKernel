using SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers;

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
        public static string Format(IDomainEventSubscriberType information)
        {
            return information.SubscriberName();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string FormatRetry(IDomainEventSubscriberType information)
        {
            return $"retry.{Format(information)}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string FormatDeadLetter(IDomainEventSubscriberType information)
        {
            return $"dead_letter.{Format(information)}";
        }
    }
}