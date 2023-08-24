using SharedKernel.Application.Documents;
using System.Globalization;

namespace SharedKernel.Infrastructure.DotNetDBF.Documents.Database;

/// <summary>  </summary>
public class DatabaseRow : IRowData
{
    private readonly List<object> _cells;
    private readonly List<string> _columnNames;
    private readonly CultureInfo _cultureInfo;

    /// <summary>  </summary>
    public DatabaseRow(long lineNumber, List<object> cells, List<string> columnNames, CultureInfo cultureInfo)
    {
        LineNumber = lineNumber;
        _cells = cells;
        _columnNames = columnNames;
        _cultureInfo = cultureInfo;
    }

    /// <summary>  </summary>
    public long LineNumber { get; }

    /// <summary>  </summary>
    public T Get<T>(int index)
    {
        if (_cells == default!)
            return default!;

        if (_cells.Count < index + 1)
            return default!;

        var cell = _cells[index];

        return GetCellValue<T>(cell);
    }

    /// <summary>  </summary>
    public T Get<T>(string name)
    {
        if (_cells == default!)
            return default!;

        var index = _columnNames.FindIndex(x => x == name);

        return Get<T>(index);
    }

    private T GetCellValue<T>(object value)
    {
        if (value == default!)
            return default!;

        return (T)Convert.ChangeType(value, typeof(T), _cultureInfo);
    }
}
