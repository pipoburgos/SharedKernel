namespace SharedKernel.Infrastructure.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public static class RabbitMqExchangeNameFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <returns></returns>
        public static string Retry(string exchangeName)
        {
            return $"retry-{exchangeName}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <returns></returns>
        public static string DeadLetter(string exchangeName)
        {
            return $"dead_letter-{exchangeName}";
        }
    }
}