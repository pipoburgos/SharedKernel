using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamHelper : IStreamHelper
    {
        /// <summary>
        /// Read the sequence and transform it into bytes
        /// Source: https://stackoverflow.com/questions/221925/creating-a-byte-array-from-a-stream
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] ToByteArray(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}
