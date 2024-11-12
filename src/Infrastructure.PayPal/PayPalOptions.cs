namespace SharedKernel.Infrastructure.PayPal;

/// <summary>
/// 
/// </summary>
public class PayPalOptions
{
    /// <summary>
    /// 
    /// </summary>
    public PayPalOptions()
    {
        Accounts = new List<Account>();
        Settings = new PayPalSettings();
    }

    /// <summary>
    /// 
    /// </summary>
    public List<Account> Accounts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PayPalSettings Settings { get; set; }
}