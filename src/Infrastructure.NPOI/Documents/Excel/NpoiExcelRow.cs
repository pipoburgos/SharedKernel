using NPOI.SS.UserModel;
using SharedKernel.Application.Documents;
using System.Globalization;

namespace SharedKernel.Infrastructure.NPOI.Documents.Excel;

/// <summary>  </summary>
public class NpoiExcelRow : IRowData
{
    private readonly IRow _cells;
    private readonly List<string> _columnNames;
    private readonly CultureInfo _cultureInfo;

    /// <summary>  </summary>
    public NpoiExcelRow(long lineNumber, IRow cells, List<string> columnNames, CultureInfo cultureInfo)
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

        var cell = _cells.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK);

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

    private T GetCellValue<T>(ICell cell)
    {
        object cellValue;

        switch (cell.CellType)
        {
            case CellType.Numeric:
                if (DateUtil.IsCellDateFormatted(cell))
                    cellValue = cell.DateCellValue ?? DateTime.MinValue;
                else
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
                cellValue = default!;
                break;
            case CellType.Blank:
                cellValue = default!;
                break;
            case CellType.Error:
                cellValue = default!;
                break;
            default:
                cellValue = default!;
                break;
        }

        if (cellValue == default)
            return default!;

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return (T)Convert.ChangeType(cellValue, typeNotNullable, _cultureInfo);
    }
}
