using SharedKernel.Application.System;
using System.Collections.Specialized;
using System.Net;

namespace SharedKernel.Infrastructure.System;

/// <summary>
/// 
/// </summary>
public class WebUtils : IWeb
{
    /// <summary> . </summary>
    public string HtmlEncode(string str)
    {
        return WebUtility.HtmlEncode(str);
    }

    /// <summary> . </summary>
    public string HtmlDecode(string str)
    {
        return WebUtility.HtmlDecode(str);
    }

    /// <summary> . </summary>
    public string UrlEncode(string str)
    {
        // ReSharper disable once RedundantSuppressNullableWarningExpression
        return WebUtility.UrlEncode(str)!;
    }

    /// <summary> . </summary>
    public string UrlDecode(string str)
    {
        return WebUtility.UrlDecode(str);
    }

    /// <summary>Parses a query string into a <see cref="T:System.Collections.Specialized.NameValueCollection" /> using the specified <see cref="T:System.Text.Encoding" />. </summary>
    /// <param name="query">The query string to parse.</param>
    /// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> of query parameters and values.</returns>
    /// <paramref name="query" /> is <see langword="null" />.- or -
    public NameValueCollection ParseQueryString(string query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (query.Length > 0 && query[0] == '?')
            query = query.Substring(1);

        var result = new NameValueCollection();
        var length = query.Length;

        var index = 0;

        while (index < length)
        {
            var startIndex = index;
            var num = -1;

            while (index < length && query[index] != '&')
            {
                if (query[index] == '=' && num < 0)
                {
                    num = index;
                }
                index++;
            }

            string? str1 = null;
            string str2;
            if (num >= 0)
            {
                str1 = query.Substring(startIndex, num - startIndex);
                str2 = query.Substring(num + 1, index - num - 1);
            }
            else
            {
                str2 = query.Substring(startIndex, index - startIndex);
            }

            result.Add(UrlDecode(str1!), UrlDecode(str2));

            // Avanzar después de '&'
            if (index < length && query[index] == '&')
            {
                index++;
                if (index == length)
                    result.Add(null, string.Empty);
            }
        }

        return result;
    }


}
