using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Entities;

// ReSharper disable UnusedParameter.Global

namespace SharedKernel.Domain.Repositories
{
    internal interface IFileRepository
    {
        Task AddAsync(FileEntity file, CancellationToken cancellationToken);

        Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);

        Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        Task<FileEntity> GetAsync(string id, CancellationToken cancellationToken);

        Task DeleteAsync(string id, CancellationToken cancellationToken);

        Task CopyAsync(FileEntity sourceFileName, FileEntity destFileName, CancellationToken cancellationToken);

        Task MoveAsync(FileEntity sourceFileName, FileEntity destFileName, CancellationToken cancellationToken);
    }
}
