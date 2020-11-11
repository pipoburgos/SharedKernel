using System.Collections.Generic;
using System.Text.RegularExpressions;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    public class StringHelper : IStringHelper
    {
        public string ToLowerUnderscore(string text)
        {
            const string rgx = @"(?x)( [A-Z][a-z,0-9]+ | [A-Z]+(?![a-z]) )";
            text = Regex.Replace(text, rgx, "_$0").ToLower();
            if (text.StartsWith("_"))
                text = text.Substring(1);

            return text;
        }

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
}
