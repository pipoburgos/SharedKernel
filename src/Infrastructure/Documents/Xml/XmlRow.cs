using SharedKernel.Application.Documents;
using SharedKernel.Domain.Extensions;
using System.Globalization;
using System.Xml.Linq;

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace SharedKernel.Infrastructure.Documents.Xml;

/// <summary> . </summary>
public class XmlRow : IRowData
{
    private readonly XElement _element;
    private readonly CultureInfo _cultureInfo;

    /// <summary> . </summary>
    public XmlRow(long lineNumber, XElement element, CultureInfo cultureInfo)
    {
        LineNumber = lineNumber;
        _element = element;
        _cultureInfo = cultureInfo;
    }

    /// <summary> . </summary>
    public long LineNumber { get; }

    /// <summary> . </summary>
    public T Get<T>(int index)
    {
        var value = _element.Descendants().ToArray()[index]?.Value ??
                    _element.Attributes().ToArray()[index]?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return default!;

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return (T)Convert.ChangeType(value, typeNotNullable, _cultureInfo);
    }

    /// <summary> . </summary>
    public Result<T> GetResult<T>(int index)
    {
        var value = _element.Descendants().ToArray()[index]?.Value ??
                    _element.Attributes().ToArray()[index]?.Value;

        if (string.IsNullOrWhiteSpace(value))
        {
            return typeof(T).IsNullable()
                ? Result.Success<T>(default!)
                : Result.Failure<T>(Error.Create("Type is required, cell is null"));
        }

        if (!value!.IsConvertible<T>())
            return Result.Failure<T>(Error.Create($"Cannot convert cell value to type '{typeof(T).Name}'."));

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return (T)Convert.ChangeType(value, typeNotNullable, _cultureInfo);

    }

    /// <summary> . </summary>
    public T Get<T>(string name)
    {
        var xmlName = name.ToPascalCase();

        var value = _element.Descendants(xmlName).SingleOrDefault()?.Value ??
                    _element.Attributes(xmlName).SingleOrDefault()?.Value;



        if (string.IsNullOrWhiteSpace(value))
            return default!;

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return (T)Convert.ChangeType(value, typeNotNullable, _cultureInfo);

    }


    /// <summary> . </summary>
    public Result<T> GetResult<T>(string name)
    {
        var value = _element.Descendants(name).SingleOrDefault()?.Value ??
                    _element.Attributes(name).SingleOrDefault()?.Value;

        if (string.IsNullOrWhiteSpace(value))
        {
            return typeof(T).IsNullable()
                ? Result.Success<T>(default!)
                : Result.Failure<T>(Error.Create("Type is required, cell is null"));
        }

        if (!value!.IsConvertible<T>())
            return Result.Failure<T>(Error.Create($"Cannot convert cell value to type '{typeof(T).Name}'."));

        var typeNotNullable = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return ((T)Convert.ChangeType(value, typeNotNullable, _cultureInfo)).Success();
    }
}