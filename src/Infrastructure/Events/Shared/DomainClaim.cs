namespace SharedKernel.Infrastructure.Events.Shared
{
    /// <summary>
    /// Domain claim
    /// </summary>
    internal class DomainClaim
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public DomainClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Claim type
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Claim value
        /// </summary>
        public string Value { get; }
    }
}