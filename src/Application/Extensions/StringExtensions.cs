using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedKernel.Application.Extensions
{
    public static class StringExtensions
    {
#pragma warning disable 1570
        /// <summary>
        ///         /// Example Usage:
        /// -------------------------------------------------------
        /// <code>
        /// var queryParams = new Dictionary<string>()
        /// {
        ///     { "x", "1" },
        ///     { "y", "2" },
        ///     { "foo", "bar" },
        ///     { "foo", "baz" },
        ///     { "special chars", "? = &" },
        /// };
        /// string url = "http://example.com/stuff" + ToQueryString(queryParams);
        /// 
        /// Console.WriteLine(url);
        /// -------------------------------------------------------
        /// Output:
        /// <string>http://example.com/stuff?x=1&y=2&foo=bar&foo=baz&special%20chars=%3F%20%3D%20%26</string>
        /// </code>
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
#pragma warning restore 1570
        public static string DictionaryToQueryString(this Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return string.Empty;

            //if (parameters == null)
            //    parameters = new Dictionary<string, object> { { "json", true } };
            //else
            //    parameters.Add("json", true);
            var sb = new StringBuilder("?");

            var first = true;

            foreach (var key in parameters.Keys.ToArray())
            {
                if (!first)
                {
                    sb.Append("&");
                }

                parameters.TryGetValue(key, out var param);
                sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(Convert.ToString(param)));

                first = false;

            }

            return first ? "" : sb.ToString();
        }

        public static string ConvertTimeSpanToString(TimeSpan time)
        {
            var formattedTimeSpan = $"{time.Hours:D2}:{time.Minutes:D2}";
            return formattedTimeSpan;
        }

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
