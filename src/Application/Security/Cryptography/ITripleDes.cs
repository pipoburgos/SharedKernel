namespace SharedKernel.Application.Security.Cryptography
{
    public interface ITripleDes
    {
        string Encrypt(string textKey, string content);
    }
}
