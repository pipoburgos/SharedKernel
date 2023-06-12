using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;

namespace SharedKernel.Infrastructure.Documents.Txt;

/// <summary>  </summary>
public class TxtRow : IRowData
{
    private readonly List<string> _cells;
    private readonly List<string> _columnNames;

    /// <summary>  </summary>
    public TxtRow(List<string> cells, List<string> columnNames)
    {
        _cells = cells;
        _columnNames = columnNames;
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

    private static T GetCellValue<T>(string cell)
    {
        if (string.IsNullOrWhiteSpace(cell))
            return default;

        return (T)Convert.ChangeType(cell, typeof(T));
    }
}
