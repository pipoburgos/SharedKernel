namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary>  </summary>
public class RabbitMqConfigParams
{
    /// <summary>  </summary>
    public string Username { get; set; }

    /// <summary>  </summary>
    public string Password { get; set; }

    /// <summary>  </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>  </summary>
    public string ExchangeName => "domain_events";

    /// <summary>  </summary>
    public string Queue { get; set; } = "CommandsQueue";

    /// <summary>  </summary>
    public int Port { get; set; } = 6379;
}
