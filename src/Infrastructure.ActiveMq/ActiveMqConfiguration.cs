namespace SharedKernel.Infrastructure.ActiveMq;

/// <summary> . </summary>
public class ActiveMqConfiguration
{
    /// <summary> . </summary>
    public string BrokerUri { get; set; } = null!;

    /// <summary> . </summary>
    public string ConsumeQueue { get; set; } = "CommandsQueue";

    /// <summary> . </summary>
    public string PublishQueue { get; set; } = "CommandsQueue";

    /// <summary> . </summary>
    public string UserName { get; set; } = null!;

    /// <summary> . </summary>
    public string Password { get; set; } = null!;
}
