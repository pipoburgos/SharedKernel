namespace SharedKernel.Application.Cqrs.Queries.DataTables;

/// <summary> . </summary>
public class Column
{
    /// <summary> . </summary>
    public Column(string data, string? name = default, Search? search = default, bool searchable = true,
        bool orderable = true)
    {
        Data = data;
        Name = name;
        Search = search;
        Searchable = searchable;
        Orderable = orderable;
    }

    /// <summary> . </summary>
    public string Data { get; }

    /// <summary> . </summary>
    public string? Name { get; }

    /// <summary> . </summary>
    public Search? Search { get; }

    /// <summary> . </summary>
    public bool Searchable { get; }

    /// <summary> . </summary>
    public bool Orderable { get; }
}
