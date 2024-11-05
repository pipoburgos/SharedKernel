namespace SharedKernel.Infrastructure.PayPal;

public class PayPalOptions
{
    public List<Account> Accounts { get; set; }

    public PayPalSettings Settings { get; set; }
}


public class PayPalSettings
{
    public string Mode { get; set; } = "sandbox";

    public int RequestRetries { get; set; } = 1;
    public int ConnectionTimeout { get; set; } = 360000;

    public string? ProxyAddress { get; set; }

    public string? ProxyCredentials { get; set; }
}

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddPayPal(this IServiceCollection services, Action<PayPalOptions> configure)
    {

    }
}
