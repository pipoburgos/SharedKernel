namespace SharedKernel.Infrastructure.PayPal;

public class PayPalOptions
{
    public List<Account> Accounts { get; set; }

    public PayPalSettings Settings { get; set; }
}