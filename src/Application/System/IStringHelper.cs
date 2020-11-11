using System.Collections.Generic;

namespace SharedKernel.Application.System
{
    public interface IStringHelper
    {
        string ToLowerUnderscore(string text);

        string Replace(string text, Dictionary<string, string> dataSource, string pattern = "##");
    }
}
