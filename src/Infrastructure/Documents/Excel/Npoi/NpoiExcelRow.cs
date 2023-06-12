using NPOI.SS.UserModel;
using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;

namespace SharedKernel.Infrastructure.Documents.Excel.Npoi;

/// <summary>  </summary>
public class NpoiExcelRow : IRowData
{
    private readonly List<ICell> _cells;
    private readonly List<string> _columnNames;

    /// <summary>  </summary>
    public NpoiExcelRow(List<ICell> cells, List<string> columnNames)
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

    private static T GetCellValue<T>(ICell cell)
    {
        object cellValue;

        switch (cell.CellType)
        {
            case CellType.Numeric:
                cellValue = cell.NumericCellValue;
                break;
            case CellType.String:
                cellValue = cell.StringCellValue;
                break;
            case CellType.Boolean:
                cellValue = cell.BooleanCellValue;
                break;
            case CellType.Formula:
                cellValue = cell.CellFormula;
                break;
            case CellType.Unknown:
                cellValue = default;
                break;
            case CellType.Blank:
                cellValue = default;
                break;
            case CellType.Error:
                cellValue = default;
                break;
            default:
                cellValue = default;
                break;
        }

        if (cellValue == default)
            return default;

        return (T)Convert.ChangeType(cellValue, typeof(T));
    }
}
