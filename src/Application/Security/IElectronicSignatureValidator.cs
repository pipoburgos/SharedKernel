namespace SharedKernel.Application.Security;

/// <summary>  </summary>
public interface IElectronicSignatureValidator
{
    /// <summary>
    /// Receive the content of a supposedly signed pdf.
    /// If a serial number is supplied, it also checks if the signature has been made
    /// with the certificate corresponding to that serial number.
    /// If a NIF is supplied, it also checks if the signature has been made
    /// with a certificate in which the Subject has that NIF.
    /// </summary>
    /// <param name="content"> PDF file content. </param>
    /// <param name="serialNumber"> String with the certificate serial number to check. </param>
    /// <param name="nif"> String with the NIF of the subject of the certificate to check. </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Validate(Stream content, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Receive the content of a supposedly signed pdf.
    /// If a serial number is supplied, it also checks if the signature has been made
    /// with the certificate corresponding to that serial number.
    /// If a NIF is supplied, it also checks if the signature has been made
    /// with a certificate in which the Subject has that NIF.
    /// </summary>
    /// <param name="content"> PDF file content. </param>
    /// <param name="serialNumber"> String with the certificate serial number to check. </param>
    /// <param name="nif"> String with the NIF of the subject of the certificate to check. </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Validate(byte[] content, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Receive the content of a supposedly signed pdf.
    /// If a serial number is supplied, it also checks if the signature has been made
    /// with the certificate corresponding to that serial number.
    /// If a NIF is supplied, it also checks if the signature has been made
    /// with a certificate in which the Subject has that NIF.
    /// </summary>
    /// <param name="path"> PDF file content. </param>
    /// <param name="serialNumber"> String with the certificate serial number to check. </param>
    /// <param name="nif"> String with the NIF of the subject of the certificate to check. </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Validate(string path, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default);

    /// <summary>  </summary>
    bool IsPdf(Stream stream);
}
