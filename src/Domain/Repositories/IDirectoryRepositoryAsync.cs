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
        /// ATENCIÓN, LEE EL CONTENIDO DE TODOS LOS FICHEROS Y LOS CARGA EN MEMORIA
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<FileEntity>> GetFilesEntitesAsync(string path, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        Task AddAsync(DirectoryEntity directory, CancellationToken cancellationToken);
    }
}
