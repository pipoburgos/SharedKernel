namespace SharedKernel.Infrastructure.Polly.RetryPolicies
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

        /// <summary>  </summary>
        public Func<int, TimeSpan> RetryAttempt()
        {
            return retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100);
        }
    }
}
