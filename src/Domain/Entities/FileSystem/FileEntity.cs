namespace SharedKernel.Domain.Entities.FileSystem;

/// <summary>  </summary>
public class FileEntity : AggregateRoot<string>
{
    /// <summary>  </summary>
    protected FileEntity() { }

    /// <summary>  </summary>
    protected FileEntity(string path, byte[] data) : base(path)
    {
        Contents = data;
    }

    /// <summary>  </summary>
    public static FileEntity Create(string path, byte[] data)
    {
        return new FileEntity(path, data);
    }

    /// <summary>  </summary>
    public static FileEntity Create(string id, string name, string extension, string contentType, byte[] contents)
    {
        return new FileEntity
        {
            Id = id,
            Name = name,
            Extension = extension,
            ContentType = contentType,
            Contents = contents
        };
    }

    /// <summary>  </summary>
    public string? Name { get; private set; }

    /// <summary>  </summary>
    public string? Extension { get; private set; }

    /// <summary>  </summary>
    public string? ContentType { get; private set; }

    /// <summary>  </summary>
    public byte[] Contents { get; private set; } = null!;

    /// <summary>  </summary>
    public DirectoryEntity? ParentDirectory { get; private set; }
}
