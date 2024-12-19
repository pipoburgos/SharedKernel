using NPOI.SS.UserModel;
using SharedKernel.Application.Documents;
using SharedKernel.Domain.Extensions;
using SharedKernel.Domain.RailwayOrientedProgramming;
using System.Globalization;

namespace SharedKernel.Infrastructure.NPOI.Documents.Excel;

/// <summary> . </summary>
public class NpoiExcelRow : IRowData
{
    private readonly IRow _cells;
    private readonly List<string> _columnNames;
    private readonly CultureInfo _cultureInfo;
    private readonly IFormulaEvaluator _formulaEvaluator;

    /// <summary> . </summary>
    public NpoiExcelRow(long lineNumber, IRow cells, List<string> columnNames, CultureInfo cultureInfo,
        IFormulaEvaluator formulaEvaluator)
    {
        LineNumber = lineNumber;
        _cells = cells;
        _columnNames = columnNames;
        _cultureInfo = cultureInfo;
        _formulaEvaluator = formulaEvaluator;
    }

    /// <summary> . </summary>
    public long LineNumber { get; }

    /// <summary> . </summary>
    public T Get<T>(int index)
    {
        if (_cells == default!)
            return default!;

        var cell = _cells.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK);

        return GetCellValue<T>(cell);
    }

    /// <summary> . </summary>
    public Result<T> GetResult<T>(int index)
    {
        if (_cells == default!)
            return default!;

        var cell = _cells.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK);

        return GetCellValueResult<T>(cell);
    }

    /// <summary> . </summary>
    public T Get<T>(string name)
    {
        if (_cells == default!)
            return default!;

        var index = _columnNames.FindIndex(x => x == name);

        return Get<T>(index);
    }

    /// <summary> . </summary>
    public Result<T> GetResult<T>(string name)
    {
        if (_cells == default!)
        {
            return Result.Success<T>(default!);
        }

        var index = _columnNames.FindIndex(x => x == name);

        if (index < 0)
            return Result.Failure<T>(Error.Create($"Column '{name}' not found."));

        return GetResult<T>(index);
    }

    private T GetCellValue<T>(ICell cell)
    {
        var cellValue = GetObjectCellValue<T>(cell);

        if (cellValue == default)
            return default!;

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return (T)Convert.ChangeType(cellValue, typeNotNullable, _cultureInfo);
    }

    private Result<T> GetCellValueResult<T>(ICell cell)
    {
        var cellValue = GetObjectCellValue<T>(cell);

        if (cellValue == default)
        {
            return typeof(T).IsNullable()
                ? Result.Success<T>(default!)
                : Result.Failure<T>(Error.Create("Type is required, cell is null"));
        }

        if (!cellValue.IsConvertible<T>())
            return Result.Failure<T>(Error.Create($"Cannot convert cell value to type '{typeof(T).Name}'."));

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return ((T)Convert.ChangeType(cellValue, typeNotNullable, _cultureInfo)).Success();
    }

    private object? GetObjectCellValue<T>(ICell cell)
    {
        object? cellValue;

        switch (cell.CellType)
        {
            case CellType.Numeric:
                if (DateUtil.IsCellDateFormatted(cell))
                    cellValue = cell.DateCellValue;
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
                cellValue = GetFormulaValue(cell);
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

        return cellValue;
    }

    private object GetFormulaValue(ICell cell)
    {
        var evaluatedValue = _formulaEvaluator.Evaluate(cell);

        switch (evaluatedValue.CellType)
        {
            case CellType.Numeric:
                if (DateUtil.IsCellDateFormatted(cell))
                    return cell.DateCellValue ?? DateTime.MinValue;

                return evaluatedValue.NumberValue;
            case CellType.String:
                return evaluatedValue.StringValue;
            case CellType.Boolean:
                return evaluatedValue.BooleanValue;
            case CellType.Blank:
                return string.Empty;
            case CellType.Error:
                return evaluatedValue.ErrorValue;
            default:
                return string.Empty;
        }
    }

}
