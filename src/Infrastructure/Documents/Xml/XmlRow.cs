using SharedKernel.Application.Documents;
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

        return (T)Convert.ChangeType(value, typeof(T), _cultureInfo);
    }

    /// <summary> . </summary>
    public T Get<T>(string name)
    {
        var value = _element.Descendants(name).SingleOrDefault()?.Value ??
                    _element.Attributes(name).SingleOrDefault()?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return default!;

        return (T)Convert.ChangeType(value, typeof(T), _cultureInfo);
    }
}