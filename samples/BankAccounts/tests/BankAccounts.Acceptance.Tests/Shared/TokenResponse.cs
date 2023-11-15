// ReSharper disable InconsistentNaming
namespace BankAccounts.Acceptance.Tests.Shared;

public class TokenResponse
{
    public TokenResponse(string access_token, int expires_in, string refresh_token)
    {
        this.access_token = access_token;
        this.expires_in = expires_in;
        this.refresh_token = refresh_token;
    }

    public string access_token { get; }

    public int expires_in { get; }

    public string refresh_token { get; }
}