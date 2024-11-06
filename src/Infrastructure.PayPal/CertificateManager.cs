using SharedKernel.Infrastructure.PayPal.Exceptions;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedKernel.Infrastructure.PayPal;

/// <summary>Manager class for storing X509 certificates.</summary>
public sealed class CertificateManager
{
    /// <summary>Cache of X509 certificates.</summary>
    private static ConcurrentDictionary<string, X509Certificate2Collection> _certificates;
    /// <summary>
    /// Private static member for storing the single instance.
    /// </summary>
    private static volatile CertificateManager _instance;
    /// <summary>
    /// Private static member for locking the singleton object while it's being instantiated.
    /// </summary>
    private static readonly object SyncRoot = new object();

    /// <summary>Private constructor prevent direct instantiation</summary>
    private CertificateManager()
    {
        _certificates = new ConcurrentDictionary<string, X509Certificate2Collection>();
    }

    /// <summary>Gets the singleton instance for this class.</summary>
    public static CertificateManager Instance
    {
        get
        {
            if (_instance == null)
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CertificateManager();
                }

            return _instance;
        }
    }

    /// <summary>
    /// Gets the certificate corresponding to the specified URL from the cache of certificates.  If the cache doesn't contain the certificate, it is downloaded and verified.
    /// </summary>
    /// <param name="certUrl">The URL pointing to the certificate.</param>
    /// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object containing the details of the certificate.</returns>
    /// <exception cref="T:SharedKernel.Infrastructure.PayPal.PayPalException">Thrown if the downloaded certificate cannot be verified.</exception>
    public X509Certificate2Collection GetCertificatesFromUrl(string certUrl)
    {
        if (!_certificates.ContainsKey(certUrl))
        {
            string str1;
            using (var webClient = new WebClient())
            {
                str1 = webClient.DownloadString(certUrl);
            }

            var strArray = str1.Split([
                "-----BEGIN CERTIFICATE-----",
                "-----END CERTIFICATE-----",
            ], StringSplitOptions.RemoveEmptyEntries);
            var certificate2Collection = new X509Certificate2Collection();
            foreach (var str2 in strArray)
            {
                var s = str2.Trim();
                if (!string.IsNullOrEmpty(s))
                {
                    var certificate = new X509Certificate2(Encoding.UTF8.GetBytes(s));
                    if (!certificate.Verify())
                        throw new PayPalException("Unable to verify the certificate(s) found at " + certUrl);

                    certificate2Collection.Add(certificate);
                }
            }
            _certificates[certUrl] = certificate2Collection;
        }
        return _certificates[certUrl];
    }

    /// <summary>
    /// Gets the trusted certificate to be used in validating a certificate chain.
    /// </summary>
    /// <param name="config">Config containing an optional path to the trusted certificate file to use.</param>
    /// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object containing the trusted certificate to use in validating a certificate chain.</returns>
    public X509Certificate2 GetTrustedCertificateFromFile(Dictionary<string, string> config)
    {
        try
        {
            if (config != null && config.TryGetValue("webhook.trustCert", out var value))
                return new X509Certificate2(File.ReadAllBytes(value));

            using (var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.crt"))
            {
                using (var destination = new MemoryStream())
                {
                    manifestResourceStream.CopyTo(destination);
                    return new X509Certificate2(destination.ToArray());
                }
            }
        }
        catch (Exception ex)
        {
            throw new PayPalException("Unable to load trusted certificate.", ex);
        }
    }

    /// <summary>
    /// Validates the certificate chain for the specified client certificate using a known, trusted certificate.
    /// </summary>
    /// <param name="trustedCert">Trusted certificate to use in validating the chain.</param>
    /// <param name="clientCerts">Client certificates to use in validating the chain.</param>
    /// <returns>True if the certificate chain is valid; false otherwise.</returns>
    public bool ValidateCertificateChain(
        X509Certificate2 trustedCert,
        X509Certificate2Collection clientCerts)
    {
        if (trustedCert == null || clientCerts == null || clientCerts.Count <= 0)
            return false;

        var x509Chain = new X509Chain();
        x509Chain.ChainPolicy.ExtraStore.AddRange(clientCerts);
        x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
        x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
        if (!x509Chain.Build(clientCerts[0]))
            return false;

        foreach (var chainElement in x509Chain.ChainElements)
        {
            if (!chainElement.Certificate.Verify())
                return false;
            if (chainElement.Certificate.Thumbprint == trustedCert.Thumbprint)
                return ValidatePayPalClientCertificate(clientCerts);
        }
        return false;
    }

    /// <summary>
    /// Validates the leaf client cert for the owner to be PayPal
    /// </summary>
    /// <param name="clientCerts"></param>
    /// <returns>True if leaf client certificate belongs to .paypal.com, false otherwise</returns>
    public bool ValidatePayPalClientCertificate(X509Certificate2Collection clientCerts)
    {
        if (clientCerts == null || clientCerts.Count <= 0)
            return false;

        // ReSharper disable once RedundantEnumerableCastCall
        var array = Regex.Matches(clientCerts[0].Subject, "CN=[a-zA-Z._-]+").Cast<Match>().Select(m => m.Value).ToArray();
        return array != null && array.Length != 0 && array[0].StartsWith("CN=") && array[0].EndsWith(".paypal.com");
    }
}