#nullable disable
using Microsoft.Win32;
using SharedKernel.Infrastructure.PayPal.Api;
using SharedKernel.Infrastructure.PayPal.Exceptions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SharedKernel.Infrastructure.PayPal.Util;

/// <summary>Helper methods for this SDK.</summary>
internal class SdkUtil
{
    /// <summary>Formats the URI path for REST calls.</summary>
    /// <param name="pattern">URI path with placeholders that can be replaced with string's Format method</param>
    /// <param name="parameters">Parameters holding actual values for placeholders; They can be wrapper objects for specific query strings like QueryParameters, CreateFromAuthorizationCodeParameters, CreateFromRefreshTokenParameters, UserinfoParameters parameters or a simple Dictionary</param>
    /// <returns>Processed URI path, or null if pattern or parameters is null</returns>
    public static string FormatUriPath(string pattern, object[] parameters)
    {
        var str = pattern;
        if (pattern != null && parameters != null)
        {
            if (parameters.Length == 1 && parameters[0] is CreateFromAuthorizationCodeParameters)
                parameters = SplitParameters(pattern, ((CreateFromAuthorizationCodeParameters)parameters[0]).ContainerMap);
            else if (parameters.Length == 1 && parameters[0] is CreateFromRefreshTokenParameters)
                parameters = SplitParameters(pattern, ((CreateFromRefreshTokenParameters)parameters[0]).ContainerMap);
            else if (parameters.Length == 1 && parameters[0] is UserinfoParameters)
                parameters = SplitParameters(pattern, ((UserinfoParameters)parameters[0]).ContainerMap);
            else if (parameters.Length == 1 && parameters[0] is Dictionary<string, string>)
                parameters = SplitParameters(pattern, (Dictionary<string, string>)parameters[0]);
            str = RemoveNullsFromQueryString(string.Format(pattern, parameters));
        }
        return str;
    }

    /// <summary>
    /// Formats the URI path for REST calls. Replaces any occurrences of the form
    /// {name} in pattern with the corresponding value of key name in the passed
    /// Dictionary
    /// </summary>
    /// <param name="pattern">URI pattern with named place holders</param>
    /// <param name="pathParameters">Dictionary</param>
    /// <returns>Processed URI path</returns>
    public static string FormatUriPath(string pattern, Dictionary<string, string> pathParameters)
    {
        return FormatUriPath(pattern, pathParameters, null);
    }

    /// <summary>
    /// Formats the URI path for REST calls. Replaces any occurrences of the form
    /// {name} in pattern with the corresponding value of key name in the passed
    /// Dictionary. Query parameters are appended to the end of the URI path
    /// </summary>
    /// <param name="pattern">URI pattern with named place holders</param>
    /// <param name="pathParameters">Dictionary of Path parameters</param>
    /// <param name="queryParameters">Dictionary for Query parameters</param>
    /// <returns>Processed URI path</returns>
    public static string FormatUriPath(
        string pattern,
        Dictionary<string, string> pathParameters,
        Dictionary<string, string> queryParameters)
    {
        if (!string.IsNullOrEmpty(pattern) && pathParameters != null && pathParameters.Count > 0)
            foreach (var pathParameter in pathParameters)
            {
                var oldValue = "{" + pathParameter.Key.Trim() + "}";
                if (pattern.Contains(oldValue))
                    pattern = pattern.Replace(oldValue, pathParameter.Value.Trim());
            }

        var str = pattern;
        if (queryParameters != null && queryParameters.Count > 0)
        {
            var stringBuilder = new StringBuilder(str);
            if (stringBuilder.ToString().Contains("?"))
            {
                if (!stringBuilder.ToString().EndsWith("?") && !stringBuilder.ToString().EndsWith("&"))
                    stringBuilder.Append("&");
            }
            else
            {
                stringBuilder.Append("?");
            }

            foreach (var queryParameter in queryParameters)
                stringBuilder.Append(HttpUtility.UrlEncode(queryParameter.Key, Encoding.UTF8)).Append("=").Append(HttpUtility.UrlEncode(queryParameter.Value, Encoding.UTF8)).Append("&");
            str = stringBuilder.ToString();
        }
        return !str.Contains("{") && !str.Contains("}") ? str : throw new PayPalException("Unable to formatURI Path : " + str + ", unable to replace placeholders with the map : " + pathParameters);
    }

    /// <summary>Removes null entries from a given query string.</summary>
    /// <param name="formatString">A query string.</param>
    /// <returns>A query string with null entries removed.</returns>
    private static string RemoveNullsFromQueryString(string formatString)
    {
        if (formatString != null && formatString.Length != 0)
        {
            var strArray1 = formatString.Split('?');
            if (strArray1.Length == 2)
            {
                var strArray2 = strArray1[1].Split('&');
                if (strArray2.Length != 0)
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var str in strArray2)
                    {
                        var strArray3 = str.Split('=');
                        if (strArray3.Length == 2 && !strArray3[1].Trim().ToLower().Equals("null") && strArray3[1].Trim().Length != 0)
                            stringBuilder.Append(str).Append("&");
                    }
                    formatString = !stringBuilder.ToString().EndsWith("&") ? stringBuilder.ToString() : stringBuilder.ToString().Substring(0, stringBuilder.ToString().Length - 1);
                }
                formatString = strArray1[0].Trim() + "?" + formatString;
            }
        }
        return formatString;
    }

    /// <summary>
    /// Split the URI and form a Object array using the query string and values
    /// in the provided map. The return object array is populated only if the map
    /// contains valid value for the query name. The object array contains null
    /// values if there is no value found in the map
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private static object[] SplitParameters(string pattern, Dictionary<string, string> parameters)
    {
        var objectList = new List<object>();
        var strArray = pattern.Split('?');
        if (strArray.Length == 2 && strArray[1].Contains("={"))
            foreach (var allKey in HttpUtility.ParseQueryString(strArray[1]).AllKeys)
            {
                string empty;
                if (parameters.TryGetValue(allKey.Trim(), out empty))
                    objectList.Add(empty);
                else
                    objectList.Add(null);
            }

        return objectList.ToArray();
    }

    /// <summary>Escapes invalid XML characters using &amp; escapes</summary>
    /// <param name="textContent">Text content to escape</param>
    /// <returns>Escaped XML string</returns>
    public static string EscapeInvalidXmlCharsRegex(string textContent)
    {
        var str = (string)null;
        if (textContent != null && textContent.Length > 0)
            str = Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(textContent, "&(?!(amp;|lt;|gt;|quot;|apos;))", "&amp;"), "<", "&lt;"), ">", "&gt;"), "\"", "&quot;"), "'", "&apos;");
        return str;
    }

    /// <summary>Escapes invalid XML characters using &amp; escapes</summary>
    /// <param name="intContent">Integer content to escape</param>
    /// <returns>Escaped XML string</returns>
    public static string EscapeInvalidXmlCharsRegex(int? intContent)
    {
        var str = (string)null;
        if (intContent.HasValue)
            str = EscapeInvalidXmlCharsRegex(intContent.ToString());
        return str;
    }

    /// <summary>Escapes invalid XML characters using &amp; escapes</summary>
    /// <param name="boolContent">Boolean content to escape</param>
    /// <returns>Escaped XML string</returns>
    public static string EscapeInvalidXmlCharsRegex(bool? boolContent)
    {
        var str = (string)null;
        if (boolContent.HasValue)
            str = EscapeInvalidXmlCharsRegex(boolContent.ToString());
        return str;
    }

    /// <summary>Escapes invalid XML characters using &amp; escapes</summary>
    /// <param name="floatContent">Float content to escape</param>
    /// <returns>Escaped XML string</returns>
    public static string EscapeInvalidXmlCharsRegex(float? floatContent)
    {
        var str = (string)null;
        if (floatContent.HasValue)
            str = EscapeInvalidXmlCharsRegex(floatContent.ToString());
        return str;
    }

    /// <summary>Escapes invalid XML characters using &amp; escapes</summary>
    /// <param name="doubleContent">Double content to escape</param>
    /// <returns>Escaped XML string</returns>
    public static string EscapeInvalidXmlCharsRegex(double? doubleContent)
    {
        var str = (string)null;
        if (doubleContent.HasValue)
            str = EscapeInvalidXmlCharsRegex(doubleContent.ToString());
        return str;
    }

    /// <summary>
    /// Gets the version number of the parent assembly for the specified object type.
    /// </summary>
    /// <param name="type">The object type to use in determining which assembly version should be returned.</param>
    /// <returns>A 3-digit version of the parent assembly.</returns>
    public static string GetAssemblyVersionForType(Type type)
    {
        return type.Assembly.GetName().Version.ToString(3);
    }

    /// <summary>
    /// Checks if .NET 4.5 or later is detected on the system.
    /// </summary>
    /// <returns>True if .NET 4.5 or later is detected; false otherwise.</returns>
    public static bool IsNet45OrLaterDetected()
    {
        var installedNetVersion = GetHighestInstalledNetVersion();
        return installedNetVersion != null && installedNetVersion >= new Version(4, 5, 0, 0);
    }

    /// <summary>
    /// Gets the highest installed version of the .NET framework found on the system.
    /// </summary>
    /// <returns>A string containing the highest installed version of the .NET framework found on the system.</returns>
    private static Version GetHighestInstalledNetVersion()
    {
        var installedNetVersion = (Version)null;
        try
        {
            using (var registryKey1 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\"))
            {
                foreach (var subKeyName1 in registryKey1.GetSubKeyNames())
                    if (subKeyName1.StartsWith("v"))
                    {
                        var registryKey2 = registryKey1.OpenSubKey(subKeyName1);
                        var version1 = registryKey2.GetValue("Version", "").ToString();
                        if (string.IsNullOrEmpty(version1))
                        {
                            foreach (var subKeyName2 in registryKey2.GetSubKeyNames())
                            {
                                var version2 = registryKey2.OpenSubKey(subKeyName2).GetValue("Version", "").ToString();
                                if (!string.IsNullOrEmpty(version2))
                                {
                                    var version3 = new Version(version2);
                                    if (installedNetVersion == null || installedNetVersion < version3)
                                        installedNetVersion = version3;
                                }
                            }
                        }
                        else
                        {
                            var version4 = new Version(version1);
                            if (installedNetVersion == null || installedNetVersion < version4)
                                installedNetVersion = version4;
                        }
                    }
            }
        }
        catch (Exception ex)
        {
        }
        return installedNetVersion;
    }

    /// <summary>
    /// Gets the resource token from an approval URL HATEOAS link, if found.
    /// </summary>
    /// <param name="links">The list of HATEOAS links objects to search.</param>
    /// <returns>A string containing the resource token associated with an approval URL.</returns>
    [Obsolete("This static method is deprecated. Call GetTokenFromApprovalUrl directly from any PayPalRelationalObject.", false)]
    public static string GetTokenFromApprovalUrl(List<Links> links)
    {
        return new PayPalRelationalObject() { Links = links }.GetTokenFromApprovalUrl();
    }
}