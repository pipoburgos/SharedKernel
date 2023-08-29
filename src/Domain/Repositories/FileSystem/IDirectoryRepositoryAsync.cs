using SharedKernel.Domain.Entities.FileSystem;

namespace SharedKernel.Domain.Repositories.FileSystem;

/// <summary>  </summary>
public interface IDirectoryRepositoryAsync
{
    /// <summary>  </summary>
    Task<List<string>> GetFilesAsync(string path, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<List<string>> GetFileNamesAsync(string path, CancellationToken cancellationToken);

    /// <summary> ATTENTION, READ THE CONTENTS OF ALL FILES AND LOAD THEM IN MEMORY. </summary>
    Task<List<FileEntity>> GetFilesEntitiesAsync(string path, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task AddAsync(DirectoryEntity directory, CancellationToken cancellationToken);
}
