using SharedKernel.Application.System;
using System.Text.RegularExpressions;

namespace SharedKernel.Infrastructure.System;

/// <summary>
/// 
/// </summary>
public class StringHelper : IStringHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string ToLowerUnderscore(string text)
    {
        const string rgx = @"(?x)( [A-Z][a-z,0-9]+ | [A-Z]+(?![a-z]) )";
        text = Regex.Replace(text, rgx, "_$0", RegexOptions.None, TimeSpan.FromMinutes(1)).ToLower();
        if (text.StartsWith("_"))
            text = text.Substring(1);

        return text;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="dataSource"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public string Replace(string text, Dictionary<string, string> dataSource, string pattern = "##")
    {
        foreach (var keyValuePair in dataSource)
        {
            var textWithPattern = $"{pattern}{keyValuePair.Key}{pattern}";
            text = text.Replace(textWithPattern, keyValuePair.Value);
        }

        return text;
    }
}