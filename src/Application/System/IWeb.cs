using System.Collections.Specialized;

namespace SharedKernel.Application.System;

/// <summary> . </summary>
public interface IWeb
{
    /// <summary> . </summary>
    string HtmlEncode(string str);

    /// <summary> . </summary>
    string HtmlDecode(string str);

    /// <summary> . </summary>
    string UrlEncode(string str);

    /// <summary> . </summary>
    string UrlDecode(string str);

    /// <summary> . </summary>
    NameValueCollection ParseQueryString(string query);
}