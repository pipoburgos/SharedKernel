using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Entities.FileSystem;
using System.Collections.Concurrent;

namespace SharedKernel.Infrastructure.Data.FileSystem.UnitOfWorks;

/// <summary>  </summary>
public class FileSystemUnitOfWork : IFileSystemUnitOfWorkAsync
{
    /// <summary>  </summary>
    public ConcurrentBag<DirectoryEntity> Directories;

    /// <summary>  </summary>
    public ConcurrentBag<FileEntity> Files;

    /// <summary>  </summary>
    public FileSystemUnitOfWork()
    {
        Directories = new ConcurrentBag<DirectoryEntity>();
        Files = new ConcurrentBag<FileEntity>();
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        foreach (var directory in Directories)
            Directory.CreateDirectory(directory.Id);

        foreach (var file in Files)
            File.WriteAllBytes(file.Id, file.Contents);

        var result = Files.Count + Directories.Count;

        Rollback();

        return result;
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return Result.Create(SaveChanges());
    }

    /// <summary>  </summary>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var directory in Directories)
            Directory.CreateDirectory(directory.Id);

        var tasks = new List<Task>();
        foreach (var file in Files)
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
            tasks.Add(File.WriteAllBytesAsync(file.Id, file.Contents, cancellationToken));
#else
            File.WriteAllBytes(file.Id, file.Contents);
#endif
        await RollbackAsync(cancellationToken);

        await Task.WhenAll(tasks);

        return await Task.FromResult(Files.Count + Directories.Count);
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Create(SaveChanges()));
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        Directories = new ConcurrentBag<DirectoryEntity>();
        Files = new ConcurrentBag<FileEntity>();
        return 0;
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Rollback());
    }
}
