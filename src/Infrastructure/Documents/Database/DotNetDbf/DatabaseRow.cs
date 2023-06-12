using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SharedKernel.Infrastructure.Documents.Database.DotNetDbf;

/// <summary>  </summary>
public class DatabaseRow : IRowData
{
    private readonly List<object> _cells;
    private readonly List<string> _columnNames;
    private readonly CultureInfo _cultureInfo;

    /// <summary>  </summary>
    public DatabaseRow(List<object> cells, List<string> columnNames, CultureInfo cultureInfo)
    {
        _cells = cells;
        _columnNames = columnNames;
        _cultureInfo = cultureInfo;
    }

    /// <summary>  </summary>
    public T Get<T>(int index)
    {
        if (_cells == default)
            return default;

        if (_cells.Count < index + 1)
            return default;

        var cell = _cells[index];

        return GetCellValue<T>(cell);
    }

    /// <summary>  </summary>
    public T Get<T>(string name)
    {
        if (_cells == default)
            return default;

        var index = _columnNames.FindIndex(x => x == name);

        return Get<T>(index);
    }

    private T GetCellValue<T>(object value)
    {
        if (value == default)
            return default;

        return (T)Convert.ChangeType(value, typeof(T), _cultureInfo);
    }
}
