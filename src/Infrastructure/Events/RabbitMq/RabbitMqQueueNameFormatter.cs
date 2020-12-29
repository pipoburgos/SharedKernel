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
        public static string Format(DomainEventSubscriberInformation information)
        {
            return information.SubscriberName();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string FormatRetry(DomainEventSubscriberInformation information)
        {
            return $"retry.{Format(information)}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string FormatDeadLetter(DomainEventSubscriberInformation information)
        {
            return $"dead_letter.{Format(information)}";
        }
    }
}