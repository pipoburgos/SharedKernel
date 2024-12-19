using SharedKernel.Application.Documents;
using SharedKernel.Domain.Extensions;
using SharedKernel.Domain.RailwayOrientedProgramming;
using System.Globalization;

namespace SharedKernel.Infrastructure.DotNetDBF.Documents.Database;

/// <summary> . </summary>
public class DatabaseRow : IRowData
{
    private readonly List<object?> _cells;
    private readonly List<string> _columnNames;
    private readonly CultureInfo _cultureInfo;

    /// <summary> . </summary>
    public DatabaseRow(long lineNumber, List<object?> cells, List<string> columnNames, CultureInfo cultureInfo)
    {
        LineNumber = lineNumber;
        _cells = cells;
        _columnNames = columnNames;
        _cultureInfo = cultureInfo;
    }

    /// <summary> . </summary>
    public long LineNumber { get; }

    /// <summary> . </summary>
    public T Get<T>(int index)
    {
        if (_cells == default!)
            return default!;

        if (_cells.Count < index + 1)
            return default!;

        var cell = _cells[index];

        return GetCellValue<T>(cell);
    }

    /// <summary> . </summary>
    public Result<T> GetResult<T>(int index)
    {
        if (_cells == default!)
            return Result.Failure<T>(Error.Create("Row is null or does not exist."));

        if (_cells.Count < index + 1)
            return Result.Failure<T>(Error.Create("Row is null or does not exist."));

        var cell = _cells[index];

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
            return typeof(T).IsNullable()
                ? default!
                : Result.Failure<T>(Error.Create("Type is required, cell is null"));
        }

        var index = _columnNames.FindIndex(x => x == name);

        if (index < 0)
            return Result.Failure<T>(Error.Create($"Column '{name}' not found."));

        return GetResult<T>(index);
    }



    private T GetCellValue<T>(object value)
    {
        if (value == default!)
            return default!;

        return (T)Convert.ChangeType(value, typeof(T), _cultureInfo);
    }

    private Result<T> GetCellValueResult<T>(object value)
    {
        if (value == default!)
            return default!;


        return !value.IsConvertible<T>()
            ? Result.Failure<T>(Error.Create($"Cannot convert cell value to type '{typeof(T).Name}'."))
            : ((T)Convert.ChangeType(value, typeof(T), _cultureInfo)).Success();
    }
}
