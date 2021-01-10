using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedKernel.Application.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
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
            if (parameters == null)
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
#if NETCOREAPP3_1 || NET5_0
                if (stringParam != null)
#endif
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(stringParam));

                first = false;

            }

            return first ? "" : sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ConvertTimeSpanToString(TimeSpan time)
        {
            var formattedTimeSpan = $"{time.Hours:D2}:{time.Minutes:D2}";
            return formattedTimeSpan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(this string str)
        {
            if (str.Length == 0)
                return str;

            if (str.Length == 1)
                return str.ToUpper();

            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
