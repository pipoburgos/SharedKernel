namespace SharedKernel.Infrastructure.PayPal;

internal sealed class PayPalTokenResponse
{
    public string Scope { get; set; } = null!;

    public string TokenType { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string AppId { get; set; } = null!;

    public int ExpiresIn { get; set; }

    public string Nonce { get; set; } = null!;
}