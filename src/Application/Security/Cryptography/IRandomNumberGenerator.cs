namespace SharedKernel.Application.Security.Cryptography
{
    public interface IRandomNumberGenerator
    {
        int GetRandom(int minValue, int maxValue);
    }
}