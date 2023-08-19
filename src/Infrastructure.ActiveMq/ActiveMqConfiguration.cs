namespace SharedKernel.Infrastructure.ActiveMq;

/// <summary>  </summary>
public class ActiveMqConfiguration
{
    /// <summary>  </summary>
    public string BrokerUri { get; set; } = null!;

    /// <summary>  </summary>
    public string Queue { get; set; } = "CommandsQueue";
}
