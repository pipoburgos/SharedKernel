namespace SharedKernel.Application.Security
{
    public class OpenIdOptions
    {
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public int AccessTokenSecondsLifetime { get; set; } = 300;
    }
}
