﻿using System.Text;

namespace SharedKernel.Application.Extensions;

/// <summary> String extensions. </summary>
public static class StringExtensions
{
    /// <summary>
    ///         /// Example Usage:
    /// -------------------------------------------------------
    /// <code>
    /// var queryParams = new Dictionary&lt;string&gt;()
    /// {
    ///     { "x", "1" },
    ///     { "y", "2" },
    ///     { "foo", "bar" },
    ///     { "foo", "baz" },
    ///     { "special chars", "? = &#38;" },
    /// };
    /// string url = "http://example.com/stuff" + ToQueryString(queryParams);
    /// 
    /// Console.WriteLine(url);
    /// -------------------------------------------------------
    /// Output:
    /// <string>http://example.com/stuff?x=1&amp;y=2&amp;foo=bar&amp;foo=baz&amp;special%20chars=%3F%20%3D%20%26</string>
    /// </code>
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static string DictionaryToQueryString(this Dictionary<string, object> parameters)
    {
        if (parameters == default!)
            return string.Empty;

        var sb = new StringBuilder("?");

        var first = true;

        foreach (var key in parameters.Keys.ToArray())
        {
            if (!first)
            {
                sb.Append('&');
            }

            parameters.TryGetValue(key, out var param);

            var stringParam = Convert.ToString(param);
#if NETCOREAPP || NET5_0 || NET6_0
            if (stringParam != null)
#endif
            sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(stringParam));

            first = false;

        }

        return first ? string.Empty : sb.ToString();
    }

    /// <summary> Converto time span to format HH:mm. </summary>
    public static string ToHoursMinutes(this TimeSpan time) => time.ToString(@"hh\:mm");

    /// <summary> Capitalize first letter. </summary>
    public static string CapitalizeFirstLetter(this string text)
    {
        switch (text.Length)
        {
            case 0:
                return text;
            case 1:
                return text.ToUpper();
            default:
                return $"{char.ToUpper(text[0])}{text.Substring(1)}";
        }
    }

    /// <summary> Remove brackets. </summary>
    public static string WithoutBrackets(this string text) =>
        new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray())
            .Normalize(NormalizationForm.FormC);
}