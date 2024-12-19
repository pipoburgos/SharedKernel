using System.Text;

namespace SharedKernel.Application.Extensions;

/// <summary> String extensions. </summary>
public static class StringExtensions
{
    /// <summary> camelCase </summary>
    public static string ToCamelCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var stringBuilder = new StringBuilder();
        var capitalizeNext = false;

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (c == '_' || c == '-' || c == ' ')
            {
                capitalizeNext = true;
            }
            else
            {
                if (capitalizeNext)
                {
                    stringBuilder.Append(char.ToUpper(c));
                    capitalizeNext = false;
                }
                else
                {
                    stringBuilder.Append(i == 0 ? char.ToLower(c) : c);
                }
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary> PascalCase. </summary>
    public static string ToPascalCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Split the text by spaces or other delimiters
        var words = text.Split([' ', '_', '-'], StringSplitOptions.RemoveEmptyEntries);

        // Capitalize each word and concatenate
        var pascalCase = string.Concat(words.Select(word =>
            char.ToUpper(word[0]) + word.Substring(1).ToLower()));

        return pascalCase;
    }

    /// <summary> kebap-case </summary>
    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if ((char.IsUpper(c) && i > 0) || c == ' ')
                stringBuilder.Append('-');

            stringBuilder.Append(char.ToLower(c));
        }

        return stringBuilder.ToString();
    }

    /// <summary> snake_case </summary>
    public static string ToSnakeCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if ((char.IsUpper(c) && i > 0) || c == ' ')
                stringBuilder.Append('_');

            stringBuilder.Append(char.ToLower(c));
        }

        return stringBuilder.ToString();
    }

    /// <summary> Train-Case  </summary>
    public static string ToTrainCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var stringBuilder = new StringBuilder();
        var capitalizeNext = true;

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (c == '_' || c == '-' || c == ' ')
            {
                stringBuilder.Append('-');
                capitalizeNext = true;
            }
            else
            {
                if (capitalizeNext)
                {
                    stringBuilder.Append(char.ToUpper(c));
                    capitalizeNext = false;
                }
                else
                {
                    stringBuilder.Append(char.ToLower(c));
                }
            }
        }

        return stringBuilder.ToString();
    }

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