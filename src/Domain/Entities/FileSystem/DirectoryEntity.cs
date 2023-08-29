namespace SharedKernel.Domain.Entities.FileSystem;

/// <summary>  </summary>
public class DirectoryEntity : AggregateRoot<string>
{
    /// <summary>  </summary>
    protected DirectoryEntity() { }

    /// <summary>  </summary>
    protected DirectoryEntity(string path, string? name) : base(path)
    {
        Name = name;
    }

    /// <summary>  </summary>
    public static DirectoryEntity Create(string path, string? name)
    {
        return new DirectoryEntity(path, name);
    }

    /// <summary>  </summary>
    public string? Name { get; protected set; }
}
