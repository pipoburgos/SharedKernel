using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    public class FileSystemUnitOfWork : IFileSystemUnitOfWorkAsync
    {
        public ConcurrentBag<DirectoryEntity> Directories;

        public ConcurrentBag<FileEntity> Files;

        public FileSystemUnitOfWork()
        {
            Directories = new ConcurrentBag<DirectoryEntity>();
            Files = new ConcurrentBag<FileEntity>();
        }

        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            Directories = new ConcurrentBag<DirectoryEntity>();
            Files = new ConcurrentBag<FileEntity>();
            return Task.FromResult(0);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var directory in Directories)
                Directory.CreateDirectory(directory.Id);

            foreach (var file in Files)
#if NETSTANDARD2_1
                File.WriteAllBytesAsync(file.Id, file.Contents, cancellationToken);
#else
                File.WriteAllBytes(file.Id, file.Contents);
#endif
            var result = Files.Count + Directories.Count;

            Directories = new ConcurrentBag<DirectoryEntity>();

            Files = new ConcurrentBag<FileEntity>();

            return Task.FromResult(result);
        }

        public int Rollback()
        {
            return RollbackAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        public int SaveChanges()
        {
            return SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
