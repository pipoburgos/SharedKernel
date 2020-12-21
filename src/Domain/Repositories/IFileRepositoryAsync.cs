using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Entities;

// ReSharper disable UnusedParameter.Global

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFileRepositoryAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(FileEntity file, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileEntity> GetAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CopyAsync(FileEntity sourceFileName, FileEntity destFileName, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task MoveAsync(FileEntity sourceFileName, FileEntity destFileName, CancellationToken cancellationToken);
    }
}
