namespace SharedKernel.Application.Security.Cryptography
{
    public interface ISecureHashAlgorithm
    {
        string Generate512String(string inputString);

        string Generate512String(byte[] bytes);

        string Generate256String(string inputString);

        string Generate256String(byte[] bytes);
    }
}
