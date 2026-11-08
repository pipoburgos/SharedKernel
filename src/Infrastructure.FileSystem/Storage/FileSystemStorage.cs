#if NET
using SharedKernel.Application.Storage;
using System.Runtime.CompilerServices;
using System.Text;

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable ArgumentsStyleOther

namespace SharedKernel.Infrastructure.FileSystem.Storage;

public sealed class FileSystemStorage : IStorage
{
    private readonly string _rootPath;
    private readonly string _rootPathWithSeparator;

    private static readonly StringComparison PathComparison =
        OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

    public FileSystemStorage(string rootPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);

        _rootPath = Path.TrimEndingDirectorySeparator(
            Path.GetFullPath(rootPath));

        _rootPathWithSeparator = Path.EndsInDirectorySeparator(_rootPath)
            ? _rootPath
            : _rootPath + Path.DirectorySeparatorChar;

        Directory.CreateDirectory(_rootPath);
    }

    // ---------------------------------------------------------
    // Directorios
    // ---------------------------------------------------------

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(ResolvePath(path, allowRoot: true));
    }

    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(ResolvePath(path, allowRoot: true));
    }

    public void DeleteDirectory(string path, bool recursive = false)
    {
        var fullPath = ResolvePath(path, allowRoot: false);

        Directory.Delete(fullPath, recursive);
    }

    public IReadOnlyList<string> GetDirectories(string path)
    {
        var fullPath = ResolvePath(path, allowRoot: true);

        return Directory
            .EnumerateDirectories(fullPath)
            .Select(ToRelativePath)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public IReadOnlyList<string> GetFiles(
        string path,
        string searchPattern = "*",
        bool recursive = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchPattern);

        if (searchPattern.IndexOfAny(
                [Path.DirectorySeparatorChar,
                 Path.AltDirectorySeparatorChar]) >= 0 ||
            searchPattern is "." or "..")
        {
            throw new ArgumentException(
                @"El patrón debe ser un nombre o patrón de fichero, no una ruta.",
                nameof(searchPattern));
        }

        var fullPath = ResolvePath(path, allowRoot: true);

        var searchOption = recursive
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

        return Directory
            .EnumerateFiles(fullPath, searchPattern, searchOption)
            .Select(ToRelativePath)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    // ---------------------------------------------------------
    // Ficheros
    // ---------------------------------------------------------

    public bool FileExists(string path)
    {
        return File.Exists(ResolvePath(path, allowRoot: false));
    }

    public void DeleteFile(string path)
    {
        File.Delete(ResolvePath(path, allowRoot: false));
    }

    public long GetFileLength(string path)
    {
        var info = new FileInfo(ResolvePath(path, allowRoot: false));

        if (!info.Exists)
            throw new FileNotFoundException(
                "No se ha encontrado el fichero.", path);

        return info.Length;
    }

    public DateTimeOffset GetLastModified(string path)
    {
        var info = new FileInfo(ResolvePath(path, allowRoot: false));

        if (!info.Exists)
            throw new FileNotFoundException(
                "No se ha encontrado el fichero.", path);

        return info.LastWriteTimeUtc;
    }

    // ---------------------------------------------------------
    // Streams
    // ---------------------------------------------------------

    public Task<Stream> OpenReadAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fullPath = ResolvePath(path, allowRoot: false);

        Stream stream = new FileStream(
            fullPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 81920,
            options: FileOptions.Asynchronous |
                     FileOptions.SequentialScan);

        return Task.FromResult(stream);
    }

    public Task<Stream> OpenWriteAsync(
        string path,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fullPath = ResolvePath(path, allowRoot: false);
        var parentDirectory = Path.GetDirectoryName(fullPath)!;

        Directory.CreateDirectory(parentDirectory);

        Stream stream = new FileStream(
            fullPath,
            overwrite ? FileMode.Create : FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            options: FileOptions.Asynchronous |
                     FileOptions.SequentialScan);

        return Task.FromResult(stream);
    }

    public async Task WriteAsync(
        string path,
        Stream source,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (!source.CanRead)
            throw new ArgumentException(
                @"El stream de origen no permite lectura.",
                nameof(source));

        try
        {
            await using var destination = await OpenWriteAsync(
                path, overwrite, cancellationToken);

            await source.CopyToAsync(
                destination,
                bufferSize: 81920,
                cancellationToken);
        }
        finally
        {
            source.Seek(0, SeekOrigin.Begin);
        }

    }

    // ---------------------------------------------------------
    // Contenido completo
    // ---------------------------------------------------------

    public async Task<string> ReadAllTextAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        await using var stream = await OpenReadAsync(
            path, cancellationToken);

        using var reader = new StreamReader(
            stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

        return await reader.ReadToEndAsync(cancellationToken);
    }

    public async Task WriteAllTextAsync(
        string path,
        string content,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        await using var stream = await OpenWriteAsync(
            path, overwrite, cancellationToken);

        await using var writer = new StreamWriter(
            stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

        await writer.WriteAsync(content.AsMemory(), cancellationToken);
    }

    public async Task<byte[]> ReadAllBytesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        await using var stream = await OpenReadAsync(
            path, cancellationToken);

        using var memory = new MemoryStream();

        await stream.CopyToAsync(memory, 81920, cancellationToken);

        return memory.ToArray();
    }

    // ---------------------------------------------------------
    // Lectura de líneas
    // ---------------------------------------------------------

    public async IAsyncEnumerable<string> ReadLinesAsync(
        string path,
        [EnumeratorCancellation]
        CancellationToken cancellationToken = default)
    {
        await using var stream = await OpenReadAsync(
            path, cancellationToken);

        using var reader = new StreamReader(
            stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

        while (true)
        {
            var line = await reader.ReadLineAsync(cancellationToken);

            if (line is null)
                yield break;

            yield return line;
        }
    }

    public async Task<IReadOnlyList<string>> ReadAllLinesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        var lines = new List<string>();

        await foreach (var line in ReadLinesAsync(path, cancellationToken))
        {
            lines.Add(line);
        }

        return lines;
    }

    // ---------------------------------------------------------
    // Búsqueda recursiva
    // ---------------------------------------------------------

    public Task<string?> FindFileAsync(
        string directory,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        if (fileName is "." or ".." ||
            fileName.IndexOfAny(
                [Path.DirectorySeparatorChar,
                 Path.AltDirectorySeparatorChar]) >= 0)
        {
            throw new ArgumentException(
                @"Indica únicamente el nombre del fichero.",
                nameof(fileName));
        }

        cancellationToken.ThrowIfCancellationRequested();

        var fullDirectory = ResolvePath(directory, allowRoot: true);

        if (!Directory.Exists(fullDirectory))
            return Task.FromResult<string?>(null);

        var file = Directory.EnumerateFiles(
                fullDirectory,
                $"{fileName}*",
                SearchOption.AllDirectories)
            .FirstOrDefault();


        return Task.FromResult(file is null ? null : ToRelativePath(file));
    }

    public Task<string?> FindFileExactAsync(string directory, string fileName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        if (fileName is "." or ".." ||
            fileName.IndexOfAny(
            [Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar]) >= 0)
        {
            throw new ArgumentException(
                @"Indica únicamente el nombre del fichero.",
                nameof(fileName));
        }

        cancellationToken.ThrowIfCancellationRequested();

        var fullDirectory = ResolvePath(directory, allowRoot: true);

        if (!Directory.Exists(fullDirectory))
            return Task.FromResult<string?>(null);

        var file = Directory.EnumerateFiles(
                fullDirectory,
                fileName,
                SearchOption.AllDirectories)
            .FirstOrDefault();


        return Task.FromResult(file is null ? null : ToRelativePath(file));
    }

    public IEnumerable<string> GetAllFilesExact(string directory, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        if (fileName is "." or ".." ||
            fileName.IndexOfAny(
            [Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar]) >= 0)
        {
            throw new ArgumentException(
                @"Indica únicamente el nombre del fichero.",
                nameof(fileName));
        }

        var fullDirectory = ResolvePath(directory, allowRoot: true);

        if (!Directory.Exists(fullDirectory))
            return Enumerable.Empty<string>();

        return Directory
            .EnumerateFiles(fullDirectory, fileName, SearchOption.AllDirectories)
            .Select(ToRelativePath);
    }

    // ---------------------------------------------------------
    // Resolución y protección de rutas
    // ---------------------------------------------------------

    private string ResolvePath(string path, bool allowRoot)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (string.IsNullOrWhiteSpace(path))
        {
            if (allowRoot)
                return _rootPath;

            throw new ArgumentException(
                @"La ruta no puede estar vacía.", nameof(path));
        }

        if (path.IndexOf('\0') >= 0 || Path.IsPathRooted(path))
            throw new ArgumentException(
                @"Solo se permiten rutas relativas a la raíz.",
                nameof(path));

        var segments = path.Split(
            ['/', '\\'],
            StringSplitOptions.RemoveEmptyEntries);

        if (segments.Any(segment => segment is "." or ".."))
            throw new ArgumentException(
                @"No se permiten segmentos '.' ni '..'.",
                nameof(path));

        var normalizedPath = path
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar);

        var fullPath = Path.GetFullPath(
            Path.Combine(_rootPath, normalizedPath));

        var isRoot = string.Equals(
            fullPath, _rootPath, PathComparison);

        if (!isRoot &&
            !fullPath.StartsWith(
                _rootPathWithSeparator, PathComparison))
        {
            throw new UnauthorizedAccessException(
                "La ruta está fuera del directorio raíz.");
        }

        if (isRoot && !allowRoot)
            throw new UnauthorizedAccessException(
                "Esta operación no se permite sobre la raíz.");

        return fullPath;
    }

    private string ToRelativePath(string fullPath)
    {
        return Path.GetRelativePath(_rootPath, fullPath)
            .Replace(Path.DirectorySeparatorChar, '/');
    }
}
#endif