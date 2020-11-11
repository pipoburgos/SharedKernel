namespace SharedKernel.Application.Security.Cryptography
{
    public interface IBase64
    {
        string EncodeTo64(string data);
        string DecodeFrom64(string data);
    }
}