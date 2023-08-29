using SharedKernel.Domain.Entities.FileSystem;

namespace SharedKernel.Domain.Repositories.FileSystem;

/// <summary>  </summary>
public interface IFileRepositoryAsync
{
    /// <summary>  </summary>
    Task AddAsync(FileEntity file, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<FileEntity> GetAsync(string id, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task DeleteAsync(string id, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task CopyAsync(FileEntity sourceFileName, FileEntity destFileName, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task MoveAsync(FileEntity sourceFileName, FileEntity destFileName, CancellationToken cancellationToken);
}
