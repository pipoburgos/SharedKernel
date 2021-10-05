namespace SharedKernel.Infrastructure.RetryPolicies
{
    /// <summary>
    /// Retriever options
    /// </summary>
    public class RetrieverOptions
    {
        /// <summary>
        /// Number of retries
        /// </summary>
        public int RetryCount { get; set; } = 5;
    }
}
