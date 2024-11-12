using System.Net;

namespace PayPal.V1.Shared.Util;

/// <summary>
/// Helper class that can be converted into a URL query string.
/// </summary>
internal class QueryParameters : Dictionary<string, string>
{
    /// <summary>
    /// Converts the dictionary of query parameters to a URL-formatted string. Empty values are ommitted from the parameter list.
    /// </summary>
    /// <returns>A URL-formatted string containing the query parameters</returns>
    public string ToUrlFormattedString()
    {
        return this.Aggregate(string.Empty,
            (parameters, item) => parameters + (string.IsNullOrEmpty(item.Value)
                ? string.Empty
                : (string.IsNullOrEmpty(parameters) ? "?" : "&") + $"{item.Key}={WebUtility.UrlEncode(item.Value)}"));
    }
}