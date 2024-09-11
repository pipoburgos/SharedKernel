namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
public class RabbitMqConfigParams
{
    /// <summary> . </summary>
    public string Username { get; set; } = null!;

    /// <summary> . </summary>
    public string Password { get; set; } = null!;

    /// <summary> . </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary> . </summary>
    public string ExchangeName => "domain_events";

    /// <summary> . </summary>
    public string ConsumeQueue { get; set; } = "CommandsQueue";

    /// <summary> . </summary>
    public string PublishQueue { get; set; } = "CommandsQueue";

    /// <summary> . </summary>
    public int Port { get; set; } = 6379;
}
