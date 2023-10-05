using iText.Kernel.Pdf;
using iText.Signatures;
using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharedKernelInfrastructure.iText;

/// <summary>  </summary>
public class ElectronicSignatureValidator : IElectronicSignatureValidator
{
    private readonly IDateTime _dateTime;

    /// <summary>  </summary>
    public ElectronicSignatureValidator(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    /// <summary>  </summary>
    public Task<bool> Validate(Stream content, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default)
    {
        if (!IsPdf(content))
        {
            return Task.FromResult(false);
        }
        using var pdfReader = new PdfReader(content);
        return Task.FromResult(Validar(pdfReader, serialNumber, nif));
    }

    /// <summary>  </summary>
    public Task<bool> Validate(byte[] content, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(content);
        if (!IsPdf(memoryStream))
        {
            return Task.FromResult(false);
        }
        using var pdfReader = new PdfReader(memoryStream);
        return Task.FromResult(Validar(pdfReader, serialNumber, nif));
    }

#if NET6_0_OR_GREATER
    /// <summary>  </summary>
    public async Task<bool> Validate(string path, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default)
    {
        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (!IsPdf(stream))
        {
            return false;
        }
        using var pdfReader = new PdfReader(stream);
        return Validar(pdfReader, serialNumber, nif);
    }
#else
    /// <summary>  </summary>
    public Task<bool> Validate(string path, string? serialNumber = default, string? nif = default,
        CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (!IsPdf(stream))
        {
            return Task.FromResult(false);
        }
        using var pdfReader = new PdfReader(stream);
        return Task.FromResult(Validar(pdfReader, serialNumber, nif));
    }

#endif
    /// <summary>  </summary>
    private bool Validar(PdfReader reader, string? serialNumber = default, string? nif = default)
    {
        var pdfDocument = new PdfDocument(reader);

        var certificados = ObtenerCertificadosValidos(pdfDocument);

        if (!certificados.Any())
            return false;

        if (serialNumber != default && certificados.All(c => c.GetSerialNumberString() != serialNumber))
            return false;

        var isValid = nif == default || certificados.Any(c => c.Subject.ToUpper().Contains($"IDCES-{nif.ToUpper()}"));

        pdfDocument.Close();

        return isValid;
    }

    /// <summary>  </summary>
    private List<X509Certificate> ObtenerCertificadosValidos(PdfDocument pdfDocument)
    {
        var signatureUtil = new SignatureUtil(pdfDocument);

        var certificados = new List<X509Certificate>();
        foreach (var name in signatureUtil.GetSignatureNames())
        {
            var pkcs7 = signatureUtil.ReadSignatureData(name);
            if (!pkcs7.VerifySignatureIntegrityAndAuthenticity())
                throw new Exception("The integrity of the signature is not intact. The signed data has been modified.");

            var pdfSign = pkcs7.GetSigningCertificate();
            pdfSign.CheckValidity(_dateTime.UtcNow);

            //if (!pdfSign.IsValid(_dateTime.UtcNow))
            //    throw new LexnetException("La hora actual no está dentro de las horas de inicio y finalización nominadas en el certificado.");

            certificados.Add(new X509Certificate(pdfSign.GetEncoded()));
        }

        return certificados;
    }

    /// <summary>  </summary>
    public bool IsPdf(Stream stream)
    {
        stream.Position = 0;
        var br = new BinaryReader(stream);
        var buffer = br.ReadBytes(5);

        var enc = new ASCIIEncoding();
        var header = enc.GetString(buffer);

        stream.Position = 0;

        //%PDF−1.0
        // If you are loading it into a long, this is (0x04034b50).
        if (buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46)
            return header.StartsWith("%PDF-");

        return false;
    }
}
