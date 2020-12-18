using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Repositories
{
    public interface IDirectoryRepositoryAsync
    {
        Task<List<string>> GetFilesAsync(string path, CancellationToken cancellationToken);

        Task<List<string>> GetFileNamesAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// ATTENTION, READ THE CONTENTS OF ALL FILES AND LOAD THEM IN MEMORY
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<List<FileEntity>> GetFilesEntitiesAsync(string path, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        Task AddAsync(DirectoryEntity directory, CancellationToken cancellationToken);
    }
}
