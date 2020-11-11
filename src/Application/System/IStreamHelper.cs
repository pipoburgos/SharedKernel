using System.IO;

namespace SharedKernel.Application.System
{
    public interface IStreamHelper
    {
        byte[] ToByteArray(Stream input);
    }
}
