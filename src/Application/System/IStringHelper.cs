using System.Collections.Generic;

namespace SharedKernel.Application.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string ToLowerUnderscore(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="dataSource"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        string Replace(string text, Dictionary<string, string> dataSource, string pattern = "##");
    }
}
