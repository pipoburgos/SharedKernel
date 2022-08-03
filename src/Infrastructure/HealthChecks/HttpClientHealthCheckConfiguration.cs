namespace SharedKernel.Infrastructure.HealthChecks
{
    /// <summary>  </summary>
    public class HttpClientHealthCheckConfiguration
    {
        /// <summary>  </summary>
        /// <param name="clientName"></param>
        /// <param name="endpoint"></param>
        public HttpClientHealthCheckConfiguration(string clientName, string endpoint)
        {
            ClientName = clientName;
            Endpoint = endpoint;
        }

        /// <summary>  </summary>
        public string ClientName { get; }

        /// <summary>  </summary>
        public string Endpoint { get; }
    }
}
