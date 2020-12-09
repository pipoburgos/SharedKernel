namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqConfigParams
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; } = "localhost";

        public string ExchangeName => "domain_events";

        public int Port { get; set; } = 6379;
    }
}