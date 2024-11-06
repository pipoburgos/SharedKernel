namespace SharedKernel.Infrastructure.PayPal;

/// <summary>Account information</summary>
public class Account
{
    /// <summary> Username. </summary>
    public string UserName { get; set; } = null!;

    /// <summary> password. </summary>
    public string Password { get; set; } = null!;

    /// <summary> Application Id. </summary>
    public string ApplicationId { get; set; } = null!;

    /// <summary> Signature. </summary>
    public string? Signature { get; set; }

    /// <summary> Client certificate for SSL authentication. </summary>
    public string? Certificate { get; set; }

    /// <summary> Private key password for SSL authentication. </summary>
    public string? PrivateKeyPassword { get; set; }

    /// <summary> Signature Subject. </summary>
    public string? SignatureSubject { get; set; }

    /// <summary> Certificate Subject. </summary>
    public string? CertificateSubject { get; set; }
}