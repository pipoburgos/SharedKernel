using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Infrastructure.Redsys.Contracts;

namespace SharedKernel.Infrastructure.Redsys.Services;
/// <summary> . </summary>
internal sealed class SignatureManager : ISignatureManager
{
    private readonly IBase64 _base64;
    private readonly ITripleDes _tripleDes;
    private readonly ISha256 _sha256;

    /// <summary> . </summary>
    public SignatureManager(IBase64 base64, ITripleDes tripleDes, ISha256 sha256)
    {
        _base64 = base64;
        _tripleDes = tripleDes;
        _sha256 = sha256;
    }

    /// <summary> . </summary>
    public string GetSignature(string merchantParameters, string merchantOrder, string merchantKey)
    {
        // Se genera una clave específica por operación. Para obtener la clave derivada a utilizar en
        // una operación se debe realizar un cifrado 3DES entre la clave del comercio y
        // el valor del número de pedido de la operación (Ds_Merchant_Order).
        var key = _base64.DecodeFrom64(merchantKey);
        var operationKey = _tripleDes.Encrypt(key, merchantOrder);

        // Se calcula el HMAC SHA256 del valor del parámetro Ds_MerchantParameters y la clave obtenida en el paso anterior.
        var hash = _sha256.HashHmac(merchantParameters, operationKey);
        return hash;
    }
}