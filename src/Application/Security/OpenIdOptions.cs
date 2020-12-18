namespace SharedKernel.Application.Security
{
    /// <summary>
    /// Authentication configuration
    /// </summary>
    public class OpenIdOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccessTokenSecondsLifetime { get; set; } = 300;
    }
}
