#if NET || NETSTANDARD2_1_OR_GREATER
namespace SharedKernel.Application.Storage;

public interface IStorage
{
    // Directorios
    bool DirectoryExists(string path);

    void CreateDirectory(string path);

    void DeleteDirectory(string path, bool recursive = false);

    IReadOnlyList<string> GetDirectories(string path);

    IReadOnlyList<string> GetFiles(
        string path,
        string searchPattern = "*",
        bool recursive = false);

    // Ficheros
    bool FileExists(string path);

    void DeleteFile(string path);

    long GetFileLength(string path);

    DateTimeOffset GetLastModified(string path);

    // Streams
    Task<Stream> OpenReadAsync(
        string path,
        CancellationToken cancellationToken = default);

    Task<Stream> OpenWriteAsync(
        string path,
        bool overwrite = false,
        CancellationToken cancellationToken = default);

    Task WriteAsync(
        string path,
        Stream source,
        bool overwrite = false,
        CancellationToken cancellationToken = default);

    // Contenido completo
    Task<string> ReadAllTextAsync(
        string path,
        CancellationToken cancellationToken = default);

    Task WriteAllTextAsync(
        string path,
        string content,
        bool overwrite = false,
        CancellationToken cancellationToken = default);

    Task<byte[]> ReadAllBytesAsync(
        string path,
        CancellationToken cancellationToken = default);

    // Líneas
    IAsyncEnumerable<string> ReadLinesAsync(
        string path,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> ReadAllLinesAsync(
        string path,
        CancellationToken cancellationToken = default);

    // Búsqueda recursiva que contenga el nombre
    Task<string?> FindFileAsync(
        string directory,
        string fileName,
        CancellationToken cancellationToken = default);

    Task<string?> FindFileExactAsync(
        string directory,
        string fileName,
        CancellationToken cancellationToken = default);

    IEnumerable<string> GetAllFilesExact(string directory, string fileName);
}

#endif