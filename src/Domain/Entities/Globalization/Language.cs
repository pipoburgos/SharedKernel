namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public class Language : AggregateRootAuditableLogicalRemove<string>
{
    /// <summary>  </summary>
    protected Language() { }

    /// <summary>  </summary>
    protected Language(string id, string name) : base(id)
    {
        Name = name;
    }

    /// <summary>  </summary>
    public static Language Create(string id, string name)
    {
        return new Language(id, name);
    }

    /// <summary>  </summary>
    public string Name { get; private set; } = null!;
}
