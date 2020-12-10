using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Data.FileSystem.UnitOfWorks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.FileSystem
{
    public class DirectoryRepositoryAsync : IDirectoryRepositoryAsync
    {
        private readonly FileSystemUnitOfWork _unitOfWork;

        public DirectoryRepositoryAsync(IFileSystemUnitOfWorkAsync unitOfWork)
        {
            _unitOfWork = unitOfWork as FileSystemUnitOfWork;
        }

        public Task AddAsync(DirectoryEntity directory, CancellationToken cancellationToken)
        {
            _unitOfWork.Directories.Add(directory);
            return Task.CompletedTask;
        }

        public Task<List<string>> GetFilesAsync(string path, CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(path).ToList();
            return Task.FromResult(files);
        }

        public Task<List<FileEntity>> GetFilesEntitesAsync(string path, CancellationToken cancellationToken)
        {
            var filesEntities = new ConcurrentBag<FileEntity>();
#if NETSTANDARD2_1
            Parallel.ForEach(Directory.GetFiles(path), async id =>
            {
                var contents = await File.ReadAllBytesAsync(path, cancellationToken);
#else
            Parallel.ForEach(Directory.GetFiles(path), id =>
            {
                var contents = File.ReadAllBytes(path);
#endif
                filesEntities.Add(FileEntity.Create(id, Path.GetFileName(id), Path.GetExtension(id),
                    MimeMappingEntity.GetMimeMapping(id), contents));
            });

            return Task.FromResult(filesEntities.ToList());
        }

        public Task<List<string>> GetFileNamesAsync(string path, CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(path).Select(Path.GetFileName).ToList();
            return Task.FromResult(files);
        }

        public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Directory.Exists(id));
        }
    }
}
