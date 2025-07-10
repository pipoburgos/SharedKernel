using System.Globalization;
using System.Text;

namespace SharedKernel.Domain.Extensions;

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

        // Separar por espacios, guiones o guiones bajos
        var words = text.Split([' ', '_', '-'], StringSplitOptions.RemoveEmptyEntries);

        // Convertir cada palabra respetando las mayúsculas existentes
        var pascalCase = string.Concat(words.Select(word =>
            char.ToUpper(word[0]) + (word.Length > 1 ? word.Substring(1) : string.Empty)));

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

            if (char.IsUpper(c) && i > 0 && text[i - 1] != '-')
                stringBuilder.Append('-');

            if (c == ' ' || c == '_')
                stringBuilder.Append('-');
            else
                stringBuilder.Append(char.ToLower(c));
        }

        return stringBuilder.ToString().Trim('-');
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

            if (char.IsWhiteSpace(c) || c == '-' || c == '_')
            {
                stringBuilder.Append('_');
            }
            else if (char.IsUpper(c))
            {
                // Verificamos manualmente si es necesario agregar '_'
                if (i > 0 && stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] != '_')
                    stringBuilder.Append('_');

                stringBuilder.Append(char.ToLower(c));
            }
            else
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Trim('_'); // Elimina guiones bajos extras al inicio o final
    }

    /// <summary> Train-Case  </summary>
    public static string ToTrainCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var stringBuilder = new StringBuilder();
        var capitalizeNext = true;

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (char.IsWhiteSpace(c) || c == '-' || c == '_')
            {
                stringBuilder.Append('-');
                capitalizeNext = true;
            }
            else if (char.IsUpper(c))
            {
                // Si no es el primer carácter y el anterior no es guion, agregamos '-'
                if (i > 0 && char.IsLower(text[i - 1]) && stringBuilder[stringBuilder.Length - 1] != '-')
                    stringBuilder.Append('-');

                stringBuilder.Append(c);
                capitalizeNext = false;
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
                    stringBuilder.Append(c);
                }
            }
        }

        return stringBuilder.ToString().Trim('-'); // Evita guiones al inicio o final
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
            if (!string.IsNullOrWhiteSpace(stringParam))
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