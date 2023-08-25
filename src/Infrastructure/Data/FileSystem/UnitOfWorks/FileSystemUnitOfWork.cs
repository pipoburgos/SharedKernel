using System.Collections.Concurrent;
using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.FileSystem.UnitOfWorks
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSystemUnitOfWork : IFileSystemUnitOfWorkAsync
    {
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentBag<DirectoryEntity> Directories;

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentBag<FileEntity> Files;

        /// <summary>
        /// 
        /// </summary>
        public FileSystemUnitOfWork()
        {
            Directories = new ConcurrentBag<DirectoryEntity>();
            Files = new ConcurrentBag<FileEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            Directories = new ConcurrentBag<DirectoryEntity>();
            Files = new ConcurrentBag<FileEntity>();
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return RollbackAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return SaveChangesAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
